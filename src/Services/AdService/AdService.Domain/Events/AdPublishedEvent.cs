using AdService.Domain.Ads.Aggregates;

namespace AdService.Domain.Events;

public record AdPublishedEvent(Ad ad) : IDomainEvent;