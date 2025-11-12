using AdService.Domain.Ads.Aggregates;
using BuildingBlocks.DDD.Abstractions;

namespace AdService.Domain.Events;

public record AdPausedEvent(Ad Ad) : IDomainEvent;