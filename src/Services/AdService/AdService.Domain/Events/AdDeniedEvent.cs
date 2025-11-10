using AdService.Domain.Ads.Aggregates;

namespace AdService.Domain.Events;

public record AdDeniedEvent(Ad ad) : IDomainEvent;