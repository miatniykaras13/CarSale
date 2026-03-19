using AdService.Contracts.Files;

namespace AdService.Application.Abstractions.FileStorage
{
    public interface IFileStorage
    {
        Task<Guid> UploadFileAsync(Stream stream, string fileName, string contentType, CancellationToken ct);

        Task<string> GetDownloadLinkAsync(Guid fileId, int expirySeconds, CancellationToken ct);

        Task<bool> DeleteFileAsync(Guid fileId, CancellationToken ct);

        Task<Guid> GenerateThumbnailAsync(Guid fileId, ThumbnailDto thumbnailDto, CancellationToken ct);
    }
}