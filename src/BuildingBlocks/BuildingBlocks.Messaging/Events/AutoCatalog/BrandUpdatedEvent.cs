namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record BrandUpdatedEvent : IntegrationEvent
{
    public required int BrandId { get; init; }

    public required string BrandName { get; init; }

    public required string Country { get; init; }

    public required int YearFrom { get; init; }

    public required int? YearTo { get; init; }
}