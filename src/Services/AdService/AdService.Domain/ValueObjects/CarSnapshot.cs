using AdService.Domain.Entities;
using AdService.Domain.Enums;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record CarSnapshot
{
    public const int REQUIRED_VIN_LENGTH = 17;

    public const int MAX_BRAND_LENGTH = 50;

    public const int MAX_MODEL_LENGTH = 50;

    public const int MAX_GENERATION_LENGTH = 50;

    public const int MAX_HORSE_POWER = 1500;

    public Guid CarId { get; private set; }

    public string Brand { get; private set; }

    public string Model { get; private set; }

    public string Generation { get; private set; }

    public int? Year { get; private set; }

    public decimal? Consumption { get; private set; }

    public int? HorsePower { get; private set; }

    public string DriveType { get; private set; }

    public string TransmissionType { get; private set; }

    public string FuelType { get; private set; }

    public string BodyType { get; private set; }

    public string? Vin { get; private set; }

    public int? Mileage { get; private set; }

    public string? Color { get; private set; }

    protected CarSnapshot()
    {
    }

    private CarSnapshot(
        Guid carId,
        string brand,
        string model,
        int? year,
        string generation,
        string? vin,
        int? mileage,
        decimal? consumption,
        string? color,
        int? horsePower,
        string driveType,
        string transmissionType,
        string fuelType,
        string bodyType)
    {
        CarId = carId;
        Brand = brand;
        Model = model;
        Year = year;
        Generation = generation;
        Vin = vin;
        Mileage = mileage;
        Color = color;
        HorsePower = horsePower;
        DriveType = driveType;
        Consumption = consumption;
        TransmissionType = transmissionType;
        FuelType = fuelType;
        BodyType = bodyType;
    }

    public static Result<CarSnapshot, Error> Of(
        Guid carId,
        string brand,
        string model,
        int? year,
        string generation,
        string? vin,
        int? mileage,
        string? color,
        int? horsePower,
        decimal? consumption,
        string driveType,
        string transmissionType,
        string fuelType,
        string bodyType)
    {
        if (year <= 1900)
        {
            return Result.Failure<CarSnapshot, Error>(Error.Domain(
                "car_snapshot.year.less_than_1900",
                "Year must be greater than 1900."));
        }

        if (vin is not null && vin.Length != REQUIRED_VIN_LENGTH)
        {
            return Result.Failure<CarSnapshot, Error>(
                Error.Domain("car_snapshot.vin.is_conflict", "Invalid vin length"));
        }

        if (horsePower is > MAX_HORSE_POWER or < 0)
        {
            return Result.Failure<CarSnapshot, Error>(
                Error.Domain(
                    "car_snapshot.horse_power.is_conflict",
                    $"Horse power must be between 0 and {MAX_HORSE_POWER}"));
        }

        CarSnapshot carSnapshot = new(carId, brand, model, year, generation, vin, mileage, consumption, color,
            horsePower,
            driveType,
            transmissionType, fuelType, bodyType);
        return Result.Success<CarSnapshot, Error>(carSnapshot);
    }
}