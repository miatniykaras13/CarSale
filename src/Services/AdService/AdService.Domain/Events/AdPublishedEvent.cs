using AdService.Domain.Ads.Aggregates;
using BuildingBlocks.DDD.Abstractions;

namespace AdService.Domain.Events;

public record AdPublishedEvent(Ad Ad) : IDomainEvent;