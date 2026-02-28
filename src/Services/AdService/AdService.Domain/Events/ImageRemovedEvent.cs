using BuildingBlocks.DDD;

namespace AdService.Domain.Events;

public record ImageRemovedEvent(Guid ImageId) : IDomainEvent;