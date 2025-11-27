namespace AdService.Application.FileStorage
{
    public interface IFileStorage
    {
        Task<Guid> UploadImageAsync(Stream stream, string fileName, string contentType, CancellationToken ct);

        Task<Guid> UploadFileAsync(Stream stream, string fileName, CancellationToken ct);

        Task<string> GetDownloadLinkAsync(Guid fileId, int expirySeconds, CancellationToken ct);

        Task<bool> DeleteFileAsync(Guid fileId, CancellationToken ct);
    }
}
