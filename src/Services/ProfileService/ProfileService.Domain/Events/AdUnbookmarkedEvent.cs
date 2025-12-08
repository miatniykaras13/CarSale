using BuildingBlocks.DDD;

namespace ProfileService.Domain.Events;

public record AdUnbookmarkedEvent(Guid AdId) : IDomainEvent;