using ProfileService.Domain.Aggregates;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Domain.Events;

public record AdSnapshotUpdatedEvent(UserProfile UserProfile, AdSnapshot AdSnapshot) : IDomainEvent;
