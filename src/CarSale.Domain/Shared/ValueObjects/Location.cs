using CSharpFunctionalExtensions;

namespace CarSale.Domain.Shared.ValueObjects;

public record Location(string Region, string City)
{
    public static Result<Location> Of(string region, string city)
    {
        if (region.Length <= 0)
        {
            return Result.Failure<Location>("Region is empty.");
        }

        if (city.Length <= 0)
        {
            return Result.Failure<Location>("City is empty.");
        }

        Location location = new(region, city);
        return Result.Success(location);
    }
}