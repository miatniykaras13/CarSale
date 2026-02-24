namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record BodyTypeUpdatedEvent : IntegrationEvent
{
    public required int BodyTypeId { get; init; }

    public required string BodyTypeName { get; init; }
}