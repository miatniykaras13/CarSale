using BuildingBlocks.DDD;
using ProfileService.Domain.Aggregates;

namespace ProfileService.Domain.Events;

public record UserCreatedEvent(User User) : IDomainEvent;