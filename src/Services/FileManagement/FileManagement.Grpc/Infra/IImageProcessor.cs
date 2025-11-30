namespace FileManagement.Grpc.Infra;

public interface IImageProcessor
{
    Task<Stream> ResizeAsync(Stream stream, int width, int height, CancellationToken ct);
}