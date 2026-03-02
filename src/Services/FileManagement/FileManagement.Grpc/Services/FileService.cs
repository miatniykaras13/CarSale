using System.Buffers;
using System.IO.Pipelines;
using FileManagement.Grpc.Data;
using FileManagement.Grpc.Infra;
using Grpc.Core;
using Minio;
using Minio.DataModel.Args;
using FileInfo = FileManagement.Grpc.Models.FileInfo;

namespace FileManagement.Grpc.Services;

public class FileService(
    FileManagementDbContext managementDbContext,
    IMinioClient minioClient,
    ILogger<FileService> logger,
    IImageProcessor imageProcessor) : FileManager.FileManagerBase
{
    public override async Task<UploadFileResponse> UploadFile(
        IAsyncStreamReader<UploadFileRequest> request,
        ServerCallContext context)
    {
        string? sourceService = null;
        string contentType = "application/octet-stream";
        string? fileName = null;
        string? bucketName = null;
        long fileSize = 0;
        var ct = context.CancellationToken;
        var fileId = Guid.CreateVersion7();

        var writerReady = new TaskCompletionSource();

        var pipe = new Pipe(new PipeOptions(
            pauseWriterThreshold: 1024 * 1024, // ~1 MB — активирует бэкпрешсур
            resumeWriterThreshold: 512 * 1024, // ~512 KB — возобновит запись
            minimumSegmentSize: 64 * 1024, // 64 KB сегменты
            useSynchronizationContext: false));

        // Буфер только для заголовка изображения
        const int headerBufferSize = 64 * 1024;
        var headerBytes = new byte[headerBufferSize];
        int headerLength = 0;

        await using var transaction = await managementDbContext.Database.BeginTransactionAsync(ct);
        try
        {
            var writer = Task.Run(
                async () =>
                {
                    await foreach (var chunk in request.ReadAllAsync(ct))
                    {
                        fileName ??= chunk.FileName;
                        sourceService ??= chunk.SourceService;

                        if (!string.IsNullOrEmpty(chunk.ContentType))
                            contentType = chunk.ContentType;

                        if (fileSize == 0 && chunk.FileSize > 0)
                            fileSize = chunk.FileSize;

                        bucketName ??= DefineBucketName(sourceService);

                        var data = chunk.Chunk;
                        if (data is not null && data.Length > 0)
                        {
                            var bytes = data.ToByteArray();
                            pipe.Writer.Write(bytes);
                            await pipe.Writer.FlushAsync(ct);

                            // Буферизируем только заголовок для определения размеров
                            if (headerLength < headerBufferSize)
                            {
                                var toWrite = Math.Min(headerBufferSize - headerLength, bytes.Length);
                                Buffer.BlockCopy(bytes, 0, headerBytes, headerLength, toWrite);
                                headerLength += toWrite;
                            }
                        }

                        if (fileName != null && contentType != null && sourceService != null && fileSize > 0)
                            writerReady.TrySetResult();
                    }

                    await pipe.Writer.CompleteAsync();
                },
                ct);

            var reader = Task.Run(
                async () =>
                {
                    await writerReady.Task;

                    await using var stream = pipe.Reader.AsStream(leaveOpen: false);

                    var objectName = GenerateObjectName(fileName, contentType, fileId, fileSize);

                    var putObjArgs = new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(fileSize)
                        .WithContentType(contentType);

                    await minioClient.PutObjectAsync(putObjArgs, ct);
                },
                ct);

            await Task.WhenAll(reader, writer);

            int width = 0;
            int height = 0;

            if (contentType.StartsWith("image") && headerLength > 0)
            {
                using var headerStream = new MemoryStream(headerBytes, 0, headerLength, writable: false);
                (width, height) = imageProcessor.GetImageSize(headerStream);
            }

            var fileInfo = FileInfo.Create(
                fileId,
                Path.GetFileNameWithoutExtension(fileName)!,
                fileSize,
                Path.GetExtension(fileName)!,
                DateTime.UtcNow,
                contentType,
                bucketName!,
                width,
                height);

            await managementDbContext.Files.AddAsync(fileInfo, ct);

            await managementDbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            return new UploadFileResponse { FileId = fileId.ToString() };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(ct);
            logger.LogError(e.Message);
            throw;
        }
    }

    public override async Task<GetDownloadLinkResponse> GetDownloadLink(
        GetDownloadLinkRequest request,
        ServerCallContext context)
    {
        var ct = context.CancellationToken;

        string bucketName = DefineBucketName(request.SourceService);

        var fileInfo = await managementDbContext.Files.FindAsync(
            [Guid.Parse(request.FileId)], ct);

        if (fileInfo is null || bucketName != fileInfo.BucketName)
            throw new RpcException(new Status(StatusCode.NotFound, $"File with id {request.FileId} not found"));

        string fileName = $"{fileInfo.Name}{fileInfo.Extension}";

        string contentType = fileInfo.ContentType;

        long fileSize = fileInfo.Size;

        var fileId = fileInfo.Id;

        string objectName =
            GenerateObjectName(fileName, contentType, fileId, fileSize, isThumbnail: fileInfo.IsThumbnail);

        int expirySeconds = request.ExpirySeconds;

        var presignedGetObjArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expirySeconds);

        string? response = await minioClient.PresignedGetObjectAsync(presignedGetObjArgs);
        return new GetDownloadLinkResponse { Link = response };
    }

    public override async Task<DeleteFileResponse> DeleteFile(
        DeleteFileRequest request,
        ServerCallContext context)
    {
        var ct = context.CancellationToken;

        var fileId = Guid.Parse(request.FileId);
        var fileInfo = await managementDbContext.Files.FindAsync([fileId], ct);

        if (fileInfo is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"File with id {fileId} not found"));

        string bucketName = DefineBucketName(request.SourceService);

        string objectName = GenerateObjectName(
            $"{fileInfo.Name}{fileInfo.Extension}",
            fileInfo.ContentType,
            fileId,
            fileInfo.Size,
            isThumbnail: fileInfo.IsThumbnail);

        await using var transaction = await managementDbContext.Database.BeginTransactionAsync(ct);
        try
        {
            if (!fileInfo.IsThumbnail)
            {
                var thumbnails = managementDbContext.Files.Where(f => f.ParentId == fileId).ToList();

                var thumbnailTasks = thumbnails.Select(async thumbnail =>
                {
                    var thumbnailObjectName = GenerateObjectName(
                        $"{thumbnail.Name}{thumbnail.Extension}",
                        thumbnail.ContentType,
                        thumbnail.Id,
                        thumbnail.Size,
                        isThumbnail: true);

                    var removeThumbnailObjArgs = new RemoveObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(thumbnailObjectName);

                    await minioClient.RemoveObjectAsync(removeThumbnailObjArgs, ct);
                });

                await Task.WhenAll(thumbnailTasks);
            }

            managementDbContext.Remove(fileInfo);

            var removeObjArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);

            await minioClient.RemoveObjectAsync(removeObjArgs, ct);
            await managementDbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return new DeleteFileResponse { IsSuccess = true };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(ct);
            logger.LogError(e.Message);
            return new DeleteFileResponse { IsSuccess = false };
        }
    }

    public override async Task<GenerateThumbnailResponse> GenerateThumbnail(
        GenerateThumbnailRequest request,
        ServerCallContext context)
    {
        var ct = context.CancellationToken;

        var fileId = Guid.Parse(request.FileId);
        var fileInfo = await managementDbContext.Files.FindAsync([fileId], ct);

        if (fileInfo is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"File with id {fileId} not found"));

        if (fileInfo.IsThumbnail)
        {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                $"Cannot generate thumbnail for thumbnail."));
        }

        if (!fileInfo.ContentType.StartsWith("image"))
        {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                $"File should be an image to generate thumbnail"));
        }

        var thumbnails = managementDbContext.Files.Where(f => f.ParentId == fileId).ToList();

        foreach (var thumbnail in thumbnails)
        {
            if (thumbnail.Width == request.ThumbnailWidth && thumbnail.Height == request.ThumbnailHeight)
                return new GenerateThumbnailResponse { ThumbnailId = thumbnail.Id.ToString() };
        }

        string bucketName = DefineBucketName(request.SourceService);

        string objectName = GenerateObjectName(
            $"{fileInfo.Name}{fileInfo.Extension}",
            fileInfo.ContentType,
            fileId,
            fileInfo.Size);

        var ms = new MemoryStream();

        var getObjArgs = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream => stream.CopyTo(ms));

        await minioClient.GetObjectAsync(getObjArgs, ct);

        ms.Position = 0;

        var thumbnailStream = await imageProcessor.ResizeAsync(
            ms,
            request.ThumbnailWidth,
            request.ThumbnailHeight,
            ct);

        var thumbnailSize = thumbnailStream.Length;

        var thumbnailId = Guid.CreateVersion7();

        var thumbnailInfo = FileInfo.Create(
            thumbnailId,
            $"{fileInfo.Name}",
            thumbnailSize,
            fileInfo.Extension,
            DateTime.UtcNow,
            fileInfo.ContentType,
            bucketName,
            request.ThumbnailWidth,
            request.ThumbnailHeight,
            parentId: fileInfo.Id);

        var thumbnailObjectName = GenerateObjectName(
            $"{fileInfo.Name}{fileInfo.Extension}",
            fileInfo.ContentType,
            thumbnailId,
            thumbnailSize,
            isThumbnail: true);

        var thumbnailPutObjArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(thumbnailObjectName)
            .WithObjectSize(thumbnailSize)
            .WithStreamData(thumbnailStream)
            .WithContentType(fileInfo.ContentType);

        await using var transaction = await managementDbContext.Database.BeginTransactionAsync(ct);
        try
        {
            await managementDbContext.Files.AddAsync(thumbnailInfo, ct);

            await minioClient.PutObjectAsync(thumbnailPutObjArgs, ct);

            await managementDbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return new GenerateThumbnailResponse { ThumbnailId = thumbnailInfo.Id.ToString(), };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(ct);
            logger.LogError(e.Message);
            throw;
        }
    }

    private string DefineBucketName(string? sourceService) =>
        sourceService switch
        {
            "AdService" => "ad-service",
            "AutoCatalog" => "auto-catalog",
            _ => "other"
        };

    private string DefineFolderByContentType(string? contentType) =>
        contentType switch
        {
            // Изображения
            "image/jpeg" => "images",
            "image/png" => "images",
            "image/gif" => "images",
            "image/webp" => "images",

            // Документы
            "application/pdf" => "documents",
            "application/msword" => "documents",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "documents",
            "application/vnd.ms-excel" => "documents",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => "documents",
            "text/plain" => "documents",
            "text/csv" => "documents",

            // Аудио
            "audio/mpeg" => "audio",
            "audio/wav" => "audio",
            "audio/ogg" => "audio",

            // Видео
            "video/mp4" => "video",
            "video/webm" => "video",
            "video/x-msvideo" => "video",

            // По умолчанию
            _ => "misc"
        };

    private string GenerateObjectName(
        string? fileName,
        string contentType,
        Guid fileId,
        long fileSize,
        bool isThumbnail = false)
    {
        string objectName =
            $"{DefineFolderByContentType(contentType)}/{fileId}_{fileSize}{Path.GetExtension(fileName)}";
        if (isThumbnail)
            objectName = GenerateThumbnailObjectName(objectName);
        return objectName;
    }

    private string GenerateThumbnailObjectName(string originalObjectName)
    {
        var words = originalObjectName.Split('/').ToList();
        if (words.Contains("thumbs"))
        {
            return originalObjectName;
        }

        words.Insert(1, $"thumbs");
        return string.Join('/', words);
    }
}