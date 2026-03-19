namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record FuelTypeUpdatedEvent : IntegrationEvent
{
    public required int FuelTypeId { get; init; }

    public required string FuelTypeName { get; init; }
}