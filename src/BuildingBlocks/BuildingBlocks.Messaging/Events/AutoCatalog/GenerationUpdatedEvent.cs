namespace BuildingBlocks.Messaging.Events.AutoCatalog;

public record GenerationUpdatedEvent : IntegrationEvent
{
    public required int GenerationId { get; init; }

    public required string GenerationName { get; init; }

    public required int ModelId { get; init; }

    public required int YearFrom { get; init; }

    public required int YearTo { get; init; }
}