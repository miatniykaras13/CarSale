using AdService.Domain.Ads.Aggregates;

namespace AdService.Domain.Abstractions;

public interface IAggregate
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    IDomainEvent[] ClearDomainEvents();
}