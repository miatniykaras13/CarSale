using System.Net.Mime;

namespace CarSale.Domain.Ads;

public class Ad
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required decimal PriceInUsd { get; set; }

    public required string Region { get; set; }

    public List<Guid> Images { get; set; } = [];

    public required Guid UserId { get; set; }

    public required Guid CarId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public List<Ad> SimilarAds { get; set; } = [];

    public List<Guid> Comments { get; set; } = [];
}