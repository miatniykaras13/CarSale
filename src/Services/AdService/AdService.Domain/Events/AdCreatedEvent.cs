using AdService.Domain.Ads.Aggregates;

namespace AdService.Domain.Events;

public record AdCreatedEvent(Ad ad) : IDomainEvent;