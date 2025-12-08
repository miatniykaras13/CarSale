using BuildingBlocks.DDD;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Domain.Events;

public record CarAddedToGarageEvent(Guid CarId) : IDomainEvent;