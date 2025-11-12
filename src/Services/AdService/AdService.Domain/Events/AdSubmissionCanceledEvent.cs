using AdService.Domain.Ads.Aggregates;
using BuildingBlocks.DDD.Abstractions;

namespace AdService.Domain.Events;

public record AdSubmissionCanceledEvent(Ad Ad) : IDomainEvent;