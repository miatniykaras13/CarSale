namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record ModelUpdatedEvent : IntegrationEvent
{
    public required int ModelId { get; init; }

    public required string ModelName { get; init; }

    public required int BrandId { get; init; }
}