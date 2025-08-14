using CarSale.Domain.Ads.Enums;
using CarSale.Domain.Ads.ValueObjects;
using CarSale.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CarSale.Domain.Ads.Entities;

public class Ad : Entity<Guid>
{
    private const int MAX_TITLE_LENGTH = 100;
    private const int MIN_TITLE_LENGTH = 10;
    private const int MAX_DESCRIPTION_LENGTH = 400;

    private Ad(
        string title,
        string description,
        Money price,
        Location location,
        long views,
        List<Guid> images,
        List<Guid> comments,
        List<Ad> similarAds,
        Guid userId,
        CarVo car,
        CarConfiguration carConfiguration,
        AdStatus status)
    {
        Id = Guid.CreateVersion7();
        Title = title;
        Description = description;
        Price = price;
        Location = location;
        Views = views;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Images = images;
        Comments = comments;
        SimilarAds = similarAds;
        UserId = userId;
        Car = car;
        CarConfiguration = carConfiguration;
        Status = status;
    }

    public Guid Id { get; }

    public string Title { get; }

    public string? Description { get; }

    public Money Price { get; }

    public Location Location { get; }

    public long Views { get; }

    public List<Guid> Images { get; }

    public Guid UserId { get; }

    public CarVo Car { get; }

    public CarConfiguration CarConfiguration { get; }

    public AdStatus Status { get; }

    public DateTime CreatedAt { get; }

    public DateTime? UpdatedAt { get; }

    public List<Ad> SimilarAds { get; }

    public List<Guid> Comments { get; }

    public static int MaxTitleLength => MAX_TITLE_LENGTH;

    public static int MinTitleLength => MIN_TITLE_LENGTH;

    public static int MaxDescriptionLength => MAX_DESCRIPTION_LENGTH;

    /*public static Result<Ad> Create( // без money
        string title,
        string description,
        string currencyCode, // для money
        decimal amount, // для money
        Location location,
        long views,
        List<Guid> images,
        List<Guid> comments,
        List<Ad> similarAds,
        Guid userId,
        CarVo car,
        CarConfiguration carConfiguration)
    {
        if (title.Length is > MAX_TITLE_LENGTH or < 0)
        {
            return Result.Failure<Ad>("Title's length is too long.");
        }

        if (description.Length is > MAX_DESCRIPTION_LENGTH or < 0)
        {
            return Result.Failure<Ad>("Description's length is too long.");
        }

        if (views < 0)
        {
            return Result.Failure<Ad>("Invalid views value.");
        }

        var moneyResult = Money.Of(currencyCode, amount);
        if (moneyResult.IsFailure)
            return Result.Failure<Ad>(moneyResult.Error);

        var priceResult = Money.Of(currencyCode, amount);
        if (priceResult.IsFailure)
            return Result.Failure<Ad>(priceResult.Error);

        return Create(title, description, priceResult.Value, location, views, images, comments, similarAds, userId,
            car, carConfiguration);

    }*/


    public static Result<Ad> Create( // готовый money
        string title,
        string description,
        long views,
        Money price,
        Location location,
        List<Guid> images,
        List<Guid> comments,
        List<Ad> similarAds,
        Guid userId,
        CarVo car,
        CarConfiguration carConfiguration,
        AdStatus status)
    {
        if (title.Length is > MAX_TITLE_LENGTH or < MIN_TITLE_LENGTH)
        {
            return Result.Failure<Ad>($"Title's length must be between {MIN_TITLE_LENGTH} and {MAX_TITLE_LENGTH}");
        }

        if (description.Length is > MAX_DESCRIPTION_LENGTH or < 0)
        {
            return Result.Failure<Ad>("Description's length is too long.");
        }

        if (views < 0)
        {
            return Result.Failure<Ad>("Invalid views value.");
        }


        return Result.Success(new Ad(title, description, price, location, views, images, comments, similarAds, userId,
            car, carConfiguration, status));
    }
}