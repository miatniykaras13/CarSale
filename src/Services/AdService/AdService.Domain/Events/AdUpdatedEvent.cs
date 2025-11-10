using AdService.Domain.Ads.Aggregates;

namespace AdService.Domain.Events;

public record AdUpdatedEvent(Ad ad) : IDomainEvent;