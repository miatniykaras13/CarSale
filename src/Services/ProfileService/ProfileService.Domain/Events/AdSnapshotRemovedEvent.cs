using ProfileService.Domain.Aggregates;

namespace ProfileService.Domain.Events;

public record AdSnapshotRemovedEvent(UserProfile UserProfile, string AdId) : IDomainEvent;
