using AdService.Domain.Entities;
using AdService.Domain.Enums;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record CarSnapshot
{
    public const int REQUIRED_VIN_LENGTH = 17;

    public Guid? CarId { get; private set; }

    public BrandSnapshot? Brand { get; private set; }

    public ModelSnapshot? Model { get; private set; }

    public GenerationSnapshot? Generation { get; private set; }

    public EngineSnapshot? Engine { get; private set; }

    public AutoDriveTypeSnapshot? DriveType { get; private set; }

    public TransmissionTypeSnapshot? TransmissionType { get; private set; }

    public BodyTypeSnapshot? BodyType { get; private set; }

    public int? Year { get; private set; }

    public decimal? Consumption { get; private set; }

    public string? Vin { get; private set; }

    public int? Mileage { get; private set; }

    public string? Color { get; private set; }

    protected CarSnapshot()
    {
    }

    private CarSnapshot(
        Guid? carId,
        BrandSnapshot? brand,
        ModelSnapshot? model,
        GenerationSnapshot? generation,
        EngineSnapshot? engine,
        AutoDriveTypeSnapshot? driveType,
        TransmissionTypeSnapshot? transmissionType,
        BodyTypeSnapshot? bodyType,
        int? year,
        string? vin,
        int? mileage,
        decimal? consumption,
        string? color)
    {
        CarId = carId;
        Brand = brand;
        Model = model;
        Generation = generation;
        Engine = engine;
        DriveType = driveType;
        TransmissionType = transmissionType;
        BodyType = bodyType;
        Year = year;
        Vin = vin;
        Mileage = mileage;
        Consumption = consumption;
        Color = color;
    }

    public static Result<CarSnapshot, Error> Of(
        Guid? carId,
        BrandSnapshot? brand,
        ModelSnapshot? model,
        GenerationSnapshot? generation,
        EngineSnapshot? engine,
        AutoDriveTypeSnapshot? driveType,
        TransmissionTypeSnapshot? transmissionType,
        BodyTypeSnapshot? bodyType,
        int? year,
        string? vin,
        int? mileage,
        decimal? consumption,
        string? color)
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

        CarSnapshot carSnapshot = new(
            carId,
            brand,
            model,
            generation,
            engine,
            driveType,
            transmissionType,
            bodyType,
            year,
            vin,
            mileage,
            consumption,
            color);
        return Result.Success<CarSnapshot, Error>(carSnapshot);
    }
}