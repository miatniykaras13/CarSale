using AdService.Application.FileStorage;
using FileManagement.Grpc;
using Microsoft.AspNetCore.Components;

namespace AdService.Infrastructure.FileStorage;

public class MinioFileStorage(FileManager.FileManagerClient client) : IFileStorage
{
    private readonly string _sourceService = "AdService";
    
    public async Task<Guid> UploadImageAsync(Stream stream, string fileName, string contentType, CancellationToken ct)
    {
        using var call = client.UploadImage(cancellationToken: ct);

        byte[] buffer = new byte[64 * 1024];

        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, ct)) > 0)
        {
            var req = new UploadImageRequest
            {
                FileName = fileName,
                ContentType = contentType,
                Chunk = Google.Protobuf.ByteString.CopyFrom(buffer, 0, bytesRead),
                SourceService = _sourceService,
            };

            await call.RequestStream.WriteAsync(req, ct);
        }

        await call.RequestStream.CompleteAsync();
        var response = await call.ResponseAsync;
        return Guid.Parse(response.FileId);
    }

    public Task<Guid> UploadFileAsync(Stream stream, string fileName, CancellationToken ct) => throw new NotImplementedException();

    public async Task<string> GetDownloadLinkAsync(Guid fileId, int expirySeconds, CancellationToken ct)
    {
        var request = new GetDownloadLinkRequest { FileId = fileId.ToString(), SourceService = _sourceService, ExpirySeconds = expirySeconds };

        var response = await client.GetDownloadLinkAsync(request, cancellationToken: ct);

        return response.Link;
    }

    public async Task<bool> DeleteFileAsync(Guid fileId, CancellationToken ct)
    {
        var request = new DeleteRequest { FileId = fileId.ToString(), SourceService = _sourceService };

        var response = await client.DeleteFileAsync(request, cancellationToken: ct);

        return response.IsSuccess;
    }
}