namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record TransmissionTypeUpdatedEvent : IntegrationEvent
{
    public required int TransmissionTypeId { get; init; }

    public required string TransmissionTypeName { get; init; }
}