using AdService.Domain.ValueObjects;
using BuildingBlocks.DDD;

namespace AdService.Domain.Events;

public record ImagesAddedEvent(IList<AdImage> Images) : IDomainEvent;