using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Domain.Cars;

public class Car
{
    public required Guid Id { get; set; }

    public required int BrandId { get; set; }

    public Brand Brand { get; set; } = null!;

    public required int ModelId { get; set; }

    public Model Model { get; set; } = null!;

    public required int GenerationId { get; set; }

    public Generation Generation { get; set; } = null!;

    public required int EngineId { get; set; }

    public Engine Engine { get; set; } = null!;

    public required TransmissionType TransmissionType { get; set; }

    public required AutoDriveType AutoDriveType { get; set; }

    public required int YearFrom { get; set; }

    public required int YearTo { get; set; }

    public required Guid PhotoId { get; set; }

    public required int Consumption { get; set; }

    public required float Acceleration { get; set; }

    public required int FuelTankCapacity { get; set; }

    public required Dimensions Dimensions { get; set; }
}