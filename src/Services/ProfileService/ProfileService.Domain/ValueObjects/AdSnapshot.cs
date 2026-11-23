using System.Text.Json.Serialization;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Domain.ValueObjects;

public record AdSnapshot
{
    public string AdId { get; private set; } = null!;

    public string Status { get; private set; } = "DRAFT";

    public string? Title { get; private set; }

    public CarSnapshot? Car { get; private set; }

    public Money? Price { get; private set; }

    protected AdSnapshot()
    {
    }

    [JsonConstructor]
    private AdSnapshot(
        string adId,
        string status,
        string? title,
        CarSnapshot? car,
        Money? price)
    {
        AdId = adId;
        Status = status;
        Title = title;
        Car = car;
        Price = price;
    }

    public static Result<AdSnapshot, Error> Of(
        string adId,
        string status,
        string? title,
        CarSnapshot? car,
        Money? price)
    {
        return Result.Success<AdSnapshot, Error>(new AdSnapshot(adId, status, title, car, price));
    }
}