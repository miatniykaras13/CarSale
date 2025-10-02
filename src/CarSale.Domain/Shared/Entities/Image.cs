using CSharpFunctionalExtensions;

namespace CarSale.Domain.Shared.Entities;

public class Image : Entity<Guid>
{
    private Image()
    {
    }

    public Image(Guid ownerId, string fileName, string contentType, long size)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        FileName = fileName;
        ContentType = contentType;
        Size = size;
        UploadedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid OwnerId { get; } // adId, carId и т.д.

    public string FileName { get; }

    public string ContentType { get; private set; }

    public long Size { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public string GetStoragePath() => $"{OwnerId}/{FileName}";
}