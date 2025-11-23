namespace BuildingBlocks.DDD;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate
    where TId : IComparable<TId>
{
    protected Aggregate()
    {
    }

    protected Aggregate(TId id)
        : base(id)
    {
    }

    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        var domainEvents = _domainEvents.ToArray();

        _domainEvents.Clear();
        return domainEvents;
    }
}