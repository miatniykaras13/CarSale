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
    public override async Task<UploadFileResponse> UploadFile(
        IAsyncStreamReader<UploadFileRequest> request,
        ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override async Task<UploadImageResponse> UploadImage(
        IAsyncStreamReader<UploadImageRequest> request,
        ServerCallContext context)
    {
        string? sourceService = null;
        string? contentType = "application/octet-stream";
        string? fileName = null;
        int fileSize = 0;
        var ct = context.CancellationToken;

        var ms = new MemoryStream();

        await foreach (var chunk in request.ReadAllAsync(ct))
        {
            ms.Write(chunk.Chunk.ToByteArray());

            fileSize += chunk.Chunk.Length;

            if (sourceService == null && !string.IsNullOrEmpty(chunk.SourceService))
                sourceService = chunk.SourceService;
            if (!string.IsNullOrEmpty(chunk.ContentType))
                contentType = chunk.ContentType;
            if (fileName == null && !string.IsNullOrEmpty(chunk.FileName))
                fileName = chunk.FileName;
        }

        ms.Seek(0, SeekOrigin.Begin);

        string bucketName = DefineBucketName(sourceService);

        var fileId = Guid.CreateVersion7();

        string objectName = $"{fileId}_{fileName}";

        var fileInfo = FileInfo.Create(
            fileId,
            Path.GetFileNameWithoutExtension(fileName)!,
            fileSize,
            Path.GetExtension(fileName)!,
            DateTime.UtcNow);

        await using var transaction = await managementDbContext.Database.BeginTransactionAsync(ct);
        try
        {
            await managementDbContext.Files.AddAsync(fileInfo, ct);

            var putObjArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(ms)
                .WithObjectSize(ms.Length)
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(putObjArgs, ct);

            await managementDbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            return new UploadImageResponse { FileId = fileId.ToString() };
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

        string objectName = $"{fileInfo.Id}_{fileInfo.Name}{fileInfo.Extension}";

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
}