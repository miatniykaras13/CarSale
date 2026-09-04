using ProfileService.Domain.Aggregates;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Domain.Events;

public record AdSnapshotAddedEvent(UserProfile UserProfile, AdSnapshot AdSnapshot) : IDomainEvent;
