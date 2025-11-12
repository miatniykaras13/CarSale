using BuildingBlocks.DDD.Abstractions;

namespace AdService.Domain.Shared.Entities;

public class Image : Entity<Guid>
{
    private Image()
        : base(Guid.CreateVersion7())
    {
    }

    public Image(Guid ownerId, string fileName, string contentType, long size)
        : this()
    {
        OwnerId = ownerId;
        FileName = fileName;
        ContentType = contentType;
        Size = size;
        UploadedAt = DateTime.UtcNow;
    }

    public Guid OwnerId { get; }

    public string FileName { get; }

    public string ContentType { get; private set; }

    public long Size { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public string GetStoragePath() => $"{OwnerId}/{FileName}";
}