namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record AutoDriveTypeUpdatedEvent : IntegrationEvent
{
    public required int AutoDriveTypeId { get; init; }

    public required string AutoDriveTypeName { get; init; }
}