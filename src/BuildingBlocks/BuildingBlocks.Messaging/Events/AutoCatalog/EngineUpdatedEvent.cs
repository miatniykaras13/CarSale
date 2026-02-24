namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record EngineUpdatedEvent : IntegrationEvent
{
    public int EngineId { get; init; }

    public required string EngineName { get; init; }

    public required int GenerationId { get; init; }

    public required float Volume { get; init; }

    public required int HorsePower { get; init; }

    public required int TorqueNm { get; init; }

    // fuel type
    public required int FuelTypeId { get; init; }

    public required string FuelTypeName { get; init; }
}