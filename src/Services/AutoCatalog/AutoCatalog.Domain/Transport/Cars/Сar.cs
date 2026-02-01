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

    public required int TransmissionTypeId { get; init; }

    public TransmissionType TransmissionType { get; init; } = null!;

    public required int DriveTypeId { get; init; }

    public AutoDriveType DriveType { get; init; } = null!;

    public required int BodyTypeId { get; init; }

    public BodyType BodyType { get; init; } = null!;

    public Guid? PhotoId { get; set; }

    public required float Consumption { get; init; }

    public required float Acceleration { get; init; }

    public required int FuelTankCapacity { get; init; }

    public required Dimensions Dimensions { get; init; }
}