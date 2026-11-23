using FileManagement.Grpc;
using Google.Protobuf;
using ProfileService.Application.Abstractions.FileStorage;

namespace ProfileService.Infrastructure.FileStorage;

public class MinioFileStorage(FileManager.FileManagerClient client) : IFileStorage
{
    private const string SourceService = "ProfileService";

    public async Task<Guid> UploadFileAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken ct)
    {
        using var call = client.UploadFile(cancellationToken: ct);

        var metadata = new UploadFileRequest
        {
            FileName = fileName,
            ContentType = contentType,
            Chunk = ByteString.Empty,
            SourceService = SourceService,
            FileSize = stream.Length,
        };

        await call.RequestStream.WriteAsync(metadata, ct);

        byte[] buffer = new byte[64 * 1024];
        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer, ct)) > 0)
        {
            var request = new UploadFileRequest
            {
                Chunk = ByteString.CopyFrom(buffer, 0, bytesRead),
            };
            await call.RequestStream.WriteAsync(request, ct);
        }

        await call.RequestStream.CompleteAsync();
        var response = await call.ResponseAsync;
        return Guid.Parse(response.FileId);
    }

    public async Task<string> GetDownloadLinkAsync(Guid fileId, int expirySeconds, CancellationToken ct)
    {
        var request = new GetDownloadLinkRequest
        {
            FileId = fileId.ToString(),
            SourceService = SourceService,
            ExpirySeconds = expirySeconds,
        };

        var response = await client.GetDownloadLinkAsync(request, cancellationToken: ct);
        return response.Link;
    }

    public async Task<bool> DeleteFileAsync(Guid fileId, CancellationToken ct)
    {
        var request = new DeleteFileRequest
        {
            FileId = fileId.ToString(),
            SourceService = SourceService,
        };

        var response = await client.DeleteFileAsync(request, cancellationToken: ct);
        return response.IsSuccess;
    }
}