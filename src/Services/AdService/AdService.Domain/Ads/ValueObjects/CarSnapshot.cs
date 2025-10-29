using CSharpFunctionalExtensions;

namespace AdService.Domain.Ads.ValueObjects;

public record CarSnapshot
{
    private const int REQUIRED_VIN_LENGTH = 17;

    public static int RequiredVinLength => REQUIRED_VIN_LENGTH;

    public string Brand { get; private set; } = null!;

    public string Model { get; private set; } = null!;

    public int Year { get; private set; }

    public string Generation { get; private set; } = null!;

    public string Vin { get; private set; }

    public int Mileage { get; private set; }

    public string Color { get; private set; } = null!;


    protected CarSnapshot()
    {
    }

    private CarSnapshot(string brand, string model, int year, string generation, string vin, int mileage, string color)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Generation = generation;
        Vin = vin;
        Mileage = mileage;
        Color = color;
    }

    public static Result<CarSnapshot, Error> Of(
        string brand,
        string model,
        int year,
        string generation,
        string vin,
        int mileage,
        string color)
    {
        if (year <= 1900)
        {
            return Result.Failure<CarSnapshot, Error>(Error.Domain("car_snapshot.year.less_than_1900",
                "Year must be greater than 1900."));
        }

        if (vin.Length != REQUIRED_VIN_LENGTH)
        {
            return Result.Failure<CarSnapshot, Error>(
                Error.Domain("car_snapshot.vin.is.conflict", "Invalid vin length"));
        }

        CarSnapshot carSnapshot = new(brand, model, year, generation, vin, mileage, color);
        return Result.Success<CarSnapshot, Error>(carSnapshot);
    }
}