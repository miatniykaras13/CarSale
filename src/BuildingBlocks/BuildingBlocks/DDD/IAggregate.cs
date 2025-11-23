namespace BuildingBlocks.DDD;

public interface IAggregate
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    IDomainEvent[] ClearDomainEvents();
}