using AdService.Domain.Aggregates;
using BuildingBlocks.DDD;

namespace AdService.Domain.Events;

public record AdPausedEvent(Ad Ad) : IDomainEvent;