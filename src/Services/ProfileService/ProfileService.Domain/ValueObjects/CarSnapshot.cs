using System.Text.Json.Serialization;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Domain.ValueObjects;

public record CarSnapshot
{
    public string Brand { get; private set; } = null!;

    public string Model { get; private set; } = null!;

    public string Generation { get; private set; } = null!;

    public int Year { get; private set; }

    public string DriveType { get; private set; } = null!;

    public string TransmissionType { get; private set; } = null!;

    public double EngineVolume { get; private set; }

    public string FuelType { get; private set; } = null!;

    public string BodyType { get; private set; } = null!;

    protected CarSnapshot()
    {
    }

    [JsonConstructor]
    private CarSnapshot(
        string brand,
        string model,
        string generation,
        int year,
        string driveType,
        string transmissionType,
        double engineVolume,
        string fuelType,
        string bodyType)
    {
        Brand = brand;
        Model = model;
        Generation = generation;
        Year = year;
        DriveType = driveType;
        TransmissionType = transmissionType;
        EngineVolume = engineVolume;
        FuelType = fuelType;
        BodyType = bodyType;
    }

    public static Result<CarSnapshot, Error> Of(
        string brand,
        string model,
        string generation,
        int year,
        string driveType,
        string transmissionType,
        double engineVolume,
        string fuelType,
        string bodyType)
    {
        return Result.Success<CarSnapshot, Error>(new CarSnapshot(
            brand,
            model,
            generation,
            year,
            driveType,
            transmissionType,
            engineVolume,
            fuelType,
            bodyType));
    }
}