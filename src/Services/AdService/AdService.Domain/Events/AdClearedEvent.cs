using AdService.Domain.Aggregates;
using BuildingBlocks.DDD.Abstractions;

namespace AdService.Domain.Events;

public record AdClearedEvent(Ad Ad) : IDomainEvent;