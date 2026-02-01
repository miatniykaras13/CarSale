using AutoCatalog.Application.Dtos;

namespace AutoCatalog.Application.Abstractions.FileStorage;

public interface IFileStorage
{
    Task<Guid> UploadLargeFileAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default);

    Task<Guid> UploadSmallFileAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default);

    Task<string> GetDownloadLinkAsync(Guid fileId, int expirySeconds, CancellationToken ct = default);

    Task<bool> DeleteFileAsync(Guid fileId, CancellationToken ct = default);

    Task<Guid> GenerateThumbnailAsync(Guid fileId, ThumbnailDto thumbnailDto, CancellationToken ct = default);
}
