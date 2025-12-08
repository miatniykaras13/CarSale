using BuildingBlocks.DDD;
using ProfileService.Domain.Aggregates;

namespace ProfileService.Domain.Events;

public record UserDeletedEvent(User User) : IDomainEvent;