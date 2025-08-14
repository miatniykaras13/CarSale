using System.Net.Mime;
using CSharpFunctionalExtensions;

namespace CarSale.Domain.Ads;

public class Ad : Entity<Guid>
{
    private const int MAX_TITLE_LENGTH = 200;
    
    private Ad(
        string title,
        string description,
        decimal priceInUsd,
        string region,
        long views,
        List<Guid> images,
        List<Guid> comments,
        List<Ad> similarAds,
        Guid userId,
        Guid carId)
    {
        Id = Guid.CreateVersion7();
        Title = title;
        Description = description;
        PriceInUsd = priceInUsd;
        Region = region;
        Views = views;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Images = images;
        Comments = comments;
        SimilarAds = similarAds;
        UserId = userId;
        CarId = carId;
    }

    public Guid Id { get; }

    public string Title { get; }

    public string? Description { get; }

    public decimal PriceInUsd { get; }

    public string Region { get; }

    public long Views { get; }

    public List<Guid> Images { get; }

    public Guid UserId { get; }

    public Guid CarId { get; }

    public DateTime CreatedAt { get; }

    public DateTime? UpdatedAt { get; }

    public List<Ad> SimilarAds { get; }

    public List<Guid> Comments { get; }

    public static Result<Ad> Of(
        string title,
        string description,
        decimal priceInUsd,
        string region,
        long views,
        List<Guid> images,
        List<Guid> comments,
        List<Ad> similarAds,
        Guid userId,
        Guid carId)
    {
        
    }
}