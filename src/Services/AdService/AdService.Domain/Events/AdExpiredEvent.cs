using AdService.Domain.Aggregates;
using BuildingBlocks.DDD.Abstractions;

namespace AdService.Domain.Events;

public record AdExpiredEvent(Ad Ad) : IDomainEvent;