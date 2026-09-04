using ProfileService.Domain.Aggregates;

namespace ProfileService.Domain.Events;

public record UserProfileUpdatedEvent(UserProfile UserProfile) : IDomainEvent;