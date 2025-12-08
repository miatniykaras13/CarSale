using BuildingBlocks.DDD;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Domain.Events;

public record CarRemovedFromGarageEvent(Guid CarId) : IDomainEvent;