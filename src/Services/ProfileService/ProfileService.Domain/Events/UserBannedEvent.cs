using BuildingBlocks.DDD;
using ProfileService.Domain.Aggregates;

namespace ProfileService.Domain.Events;

public record UserBannedEvent(User User) : IDomainEvent;