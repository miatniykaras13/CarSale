using BuildingBlocks.DDD;

namespace ProfileService.Domain.Events;

public record AdBookmarkedEvent(Guid AdId) : IDomainEvent;