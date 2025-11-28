namespace AdService.Application.FileStorage
{
    public interface IFileStorage
    {
        Task<Guid> UploadLargeFileAsync(Stream stream, string fileName, string contentType, CancellationToken ct);

        Task<Guid> UploadSmallFileAsync(Stream stream, string fileName, string contentType, CancellationToken ct);

        Task<string> GetDownloadLinkAsync(Guid fileId, int expirySeconds, CancellationToken ct);

        Task<bool> DeleteFileAsync(Guid fileId, CancellationToken ct);
    }
}
