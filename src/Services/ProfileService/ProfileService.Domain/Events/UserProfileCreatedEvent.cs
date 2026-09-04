using ProfileService.Domain.Aggregates;

namespace ProfileService.Domain.Events;

public record UserProfileCreatedEvent(UserProfile UserProfile) : IDomainEvent;
