using System.Buffers;
using System.Diagnostics.Tracing;
using System.IO.Pipelines;
using FileManagement.Grpc.Data;
using Grpc.Core;
using Minio;
using Minio.DataModel.Args;
using FileInfo = FileManagement.Grpc.Models.FileInfo;

namespace FileManagement.Grpc.Services;

public class FileService(
    FileManagementDbContext managementDbContext,
    IMinioClient minioClient,
    ILogger<FileService> logger) : FileManager.FileManagerBase
{
    public override async Task<UploadSmallFileResponse> UploadSmallFile(
        UploadSmallFileRequest request,
        ServerCallContext context)
    {
        var ct = context.CancellationToken;

        var ms = new MemoryStream(request.File.ToByteArray());

        string bucketName = DefineBucketName(request.SourceService);

        string contentType = request.ContentType!;

        string fileName = request.FileName!;

        var fileId = Guid.CreateVersion7();

        string objectName = GenerateObjectName(fileName, contentType, fileId);

        long fileSize = request.File.Length;

        var fileInfo = FileInfo.Create(
            fileId,
            Path.GetFileNameWithoutExtension(fileName)!,
            fileSize,
            Path.GetExtension(fileName),
            DateTime.UtcNow,
            contentType);

        var putObjArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(ms)
            .WithObjectSize(fileSize)
            .WithContentType(contentType);

        await using var transaction = await managementDbContext.Database.BeginTransactionAsync(ct);
        try
        {
            await managementDbContext.Files.AddAsync(fileInfo, ct);

            await minioClient.PutObjectAsync(putObjArgs, ct);

            await managementDbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            return new UploadSmallFileResponse { FileId = fileId.ToString() };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(ct);
            logger.LogError(e.Message);
            throw;
        }
    }

    public override async Task<UploadLargeFileResponse> UploadLargeFile(
        IAsyncStreamReader<UploadLargeFileRequest> request,
        ServerCallContext context)
    {
        string? sourceService = null;
        string? contentType = "application/octet-stream";
        string? fileName = null;
        long fileSize = 0;
        var ct = context.CancellationToken;
        var fileId = Guid.CreateVersion7();

        var writerReady = new TaskCompletionSource();

        var pipe = new Pipe();

        await using var transaction = await managementDbContext.Database.BeginTransactionAsync(ct);
        try
        {
            var writer = Task.Run(async () =>
            {
                await foreach (var chunk in request.ReadAllAsync(ct))
                {
                    fileName ??= chunk.FileName;
                    sourceService ??= chunk.SourceService;

                    if (!string.IsNullOrEmpty(chunk.ContentType))
                        contentType = chunk.ContentType;

                    if (fileSize == 0 && chunk.FileSize > 0)
                        fileSize = chunk.FileSize;

                    var data = chunk.Chunk;
                    if (data is not null && data.Length > 0)
                    {
                        var span = data.Span;
                        pipe.Writer.Write(span);
                        await pipe.Writer.FlushAsync(ct);
                    }

                    if (fileName != null && contentType != null && sourceService != null && fileSize > 0)
                        writerReady.TrySetResult();
                }

                await pipe.Writer.CompleteAsync();
            });

            var reader = Task.Run(async () =>
            {
                await writerReady.Task;

                await using var stream = pipe.Reader.AsStream(leaveOpen: false);

                var bucketName = DefineBucketName(sourceService);

                var objectName = GenerateObjectName(fileName, contentType, fileId);

                var putObjArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(fileSize)
                    .WithContentType(contentType);

                await minioClient.PutObjectAsync(putObjArgs, ct);
            });


            await Task.WhenAll(reader, writer);

            var fileInfo = FileInfo.Create(
                fileId,
                Path.GetFileNameWithoutExtension(fileName)!,
                fileSize,
                Path.GetExtension(fileName)!,
                DateTime.UtcNow,
                contentType);

            await managementDbContext.Files.AddAsync(fileInfo, ct);

            await managementDbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            return new UploadLargeFileResponse { FileId = fileId.ToString() };
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

        var fileInfo = await managementDbContext.Files.FindAsync(
            [Guid.Parse(request.FileId)], ct);

        if (fileInfo is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"File with id {request.FileId} not found"));

        string bucketName = DefineBucketName(request.SourceService);

        string fileName = $"{fileInfo.Name}{fileInfo.Extension}";

        string contentType = fileInfo.ContentType;

        var fileId = fileInfo.Id;

        string objectName = GenerateObjectName(fileName, contentType, fileId);

        int expirySeconds = request.ExpirySeconds;

        var presignedGetObjArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expirySeconds);

        string? response = await minioClient.PresignedGetObjectAsync(presignedGetObjArgs);
        return new GetDownloadLinkResponse { Link = response };
    }

    public override async Task<DeleteResponse> DeleteFile(
        DeleteRequest request,
        ServerCallContext context)
    {
        var ct = context.CancellationToken;

        var fileId = Guid.Parse(request.FileId);
        var fileInfo = await managementDbContext.Files.FindAsync([fileId], ct);

        if (fileInfo is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"File with id {fileId} not found"));

        string bucketName = DefineBucketName(request.SourceService);

        string objectName = $"{fileInfo.Id}_{fileInfo.Name}{fileInfo.Extension}";

        await using var transaction = await managementDbContext.Database.BeginTransactionAsync(ct);
        try
        {
            managementDbContext.Remove(fileInfo);

            var removeObjArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);

            await minioClient.RemoveObjectAsync(removeObjArgs, ct);
            await managementDbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return new DeleteResponse { IsSuccess = true };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(ct);
            logger.LogError(e.Message);
            return new DeleteResponse { IsSuccess = false };
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


    private string GenerateObjectName(string? fileName, string contentType, Guid fileId) =>
        $"{DefineFolderByContentType(contentType)}/{fileId}_{fileName ?? "file"}";
}