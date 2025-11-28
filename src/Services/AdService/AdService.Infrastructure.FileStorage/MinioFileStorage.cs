using AdService.Application.FileStorage;
using FileManagement.Grpc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;

namespace AdService.Infrastructure.FileStorage;

public class MinioFileStorage(FileManager.FileManagerClient client) : IFileStorage
{
    private readonly string _sourceService = "AdService";


    public async Task<Guid> UploadLargeFileAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken ct)
    {
        using var call = client.UploadLargeFile(cancellationToken: ct);

        byte[] buffer = new byte[64 * 1024];

        int bytesRead;


        var metadata = new UploadLargeFileRequest
        {
            FileName = fileName,
            ContentType = contentType,
            Chunk = Google.Protobuf.ByteString.Empty,
            SourceService = _sourceService,
            FileSize = stream.Length,
        };

        await call.RequestStream.WriteAsync(metadata, ct);

        while ((bytesRead = await stream.ReadAsync(buffer, ct)) > 0)
        {
            var req = new UploadLargeFileRequest { Chunk = Google.Protobuf.ByteString.CopyFrom(buffer, 0, bytesRead), };

            await call.RequestStream.WriteAsync(req, ct);
        }

        await call.RequestStream.CompleteAsync();
        var response = await call.ResponseAsync;
        return Guid.Parse(response.FileId);
    }

    public async Task<Guid> UploadSmallFileAsync(Stream stream, string fileName, string contentType,
        CancellationToken ct)
    {
        var request = new UploadSmallFileRequest()
        {
            FileName = fileName,
            ContentType = contentType,
            File = await Google.Protobuf.ByteString.FromStreamAsync(stream, ct),
            SourceService = _sourceService,
            FileSize = stream.Length,
        };


        var response = await client.UploadSmallFileAsync(request, cancellationToken: ct);
        return Guid.Parse(response.FileId);
    }

    public async Task<string> GetDownloadLinkAsync(Guid fileId, int expirySeconds, CancellationToken ct)
    {
        var request = new GetDownloadLinkRequest
        {
            FileId = fileId.ToString(), SourceService = _sourceService, ExpirySeconds = expirySeconds,
        };

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