using System.Text.Json.Serialization;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Domain.Transport.Cars;

public class Car
{
    public required Guid Id { get; init; }

    public required int BrandId { get; init; }

    public Brand Brand { get; init; } = null!;

    public required int ModelId { get; init; }

    public Model Model { get; init; } = null!;

    public required int GenerationId { get; init; }

    public Generation Generation { get; init; } = null!;

    public required int EngineId { get; init; }

    public Engine Engine { get; init; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required TransmissionType TransmissionType { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required AutoDriveType AutoDriveType { get; init; }

    public required int YearFrom { get; init; }

    public required int YearTo { get; init; }

    public required Guid PhotoId { get; init; }

    public required decimal Consumption { get; init; }

    public required decimal Acceleration { get; init; }

    public required int FuelTankCapacity { get; init; }

    public required Dimensions Dimensions { get; init; }
}