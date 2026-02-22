namespace BuildingBlocks.Messaging.Events;

public record BrandUpdatedEvent : IntegrationEvent
{
    public required int BrandId { get; init; }

    public required string BrandName { get; init; }
}