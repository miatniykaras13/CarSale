using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Domain.Enums;

namespace ProfileService.Domain.ValueObjects;

public record CarSnapshot
{
    public const int REQUIRED_VIN_LENGTH = 17;

    public const int MAX_HORSE_POWER = 1500;

    public Guid CarId { get; private set; }

    public string Brand { get; private set; } = null!;

    public string Model { get; private set; } = null!;

    public string Generation { get; private set; } = null!;

    public int Year { get; private set; }

    public decimal? Consumption { get; private set; }

    public int HorsePower { get; private set; }

    public AutoDriveType DriveType { get; private set; }

    public TransmissionType TransmissionType { get; private set; }

    public FuelType FuelType { get; private set; }

    public string? Vin { get; private set; }

    public int? Mileage { get; private set; }

    public string? Color { get; private set; }


    protected CarSnapshot()
    {
    }

    private CarSnapshot(Guid carId, string brand, string model, int year, string generation, string? vin, int? mileage,
        decimal? consumption,
        string? color, int horsePower, AutoDriveType driveType, TransmissionType transmissionType, FuelType fuelType)
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
    }

    public static Result<CarSnapshot, Error> Of(
        Guid carId,
        string brand,
        string model,
        int year,
        string generation,
        string? vin,
        int mileage,
        string color,
        int horsePower,
        decimal consumption,
        AutoDriveType driveType,
        TransmissionType transmissionType,
        FuelType fuelType)
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
            transmissionType, fuelType);
        return Result.Success<CarSnapshot, Error>(carSnapshot);
    }
}