using CarSale.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CarSale.Domain.Shared.Entities;

public class Image : Entity<Guid>
{
    public Guid Id { get; private set; }

    public Guid OwnerId { get; private set; } // adId, carId и т.д.

    public string FileName { get; private set; }

    public string ContentType { get; private set; }

    public long Size { get; private set; }

    public DateTime UploadedAt { get; private set; }

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

    public string GetStoragePath()
    {
        return $"{OwnerId}/{FileName}";
    }
}