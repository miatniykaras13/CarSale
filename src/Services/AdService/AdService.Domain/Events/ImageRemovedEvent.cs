using AdService.Domain.ValueObjects;
using BuildingBlocks.DDD;

namespace AdService.Domain.Events;

public record ImageRemovedEvent(AdImage Image) : IDomainEvent;