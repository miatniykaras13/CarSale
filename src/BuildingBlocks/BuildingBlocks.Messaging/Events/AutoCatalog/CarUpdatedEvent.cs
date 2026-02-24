namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record CarUpdatedEvent : IntegrationEvent
{
    public required Guid CarId { get; init; }

    public required int BrandId { get; init; }

    public required int ModelId { get; init; }

    public required int GenerationId { get; init; }

    public required int EngineId { get; init; }

    public required int TransmissionTypeId { get; init; }

    public required int DriveTypeId { get; init; }

    public required int BodyTypeId { get; init; }

    public required float Consumption { get; init; }

    public required float Acceleration { get; init; }

    public required int FuelTankCapacity { get; init; }


    // dimensions
    public required int Length { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
}