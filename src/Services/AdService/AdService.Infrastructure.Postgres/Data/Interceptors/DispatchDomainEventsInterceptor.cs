using BuildingBlocks.DDD;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdService.Infrastructure.Postgres.Data.Interceptors;


// todo сделать, чтоб не было гонок либо реализовать outbox pattern
public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    private List<IDomainEvent> _domainEvents = []; // это заменить

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        _domainEvents = CollectAndClearDomainEventsAsync(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        _domainEvents = await CollectAndClearDomainEventsAsync(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, ct);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        _domainEvents.Clear();
        base.SaveChangesFailed(eventData);
    }

    public override async Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken ct = default)
    {
        _domainEvents.Clear();
        await base.SaveChangesFailedAsync(eventData, ct);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken ct = default)
    {
        foreach (var domainEvent in _domainEvents)
            await mediator.Publish(domainEvent, ct);
        _domainEvents.Clear();
        return await base.SavedChangesAsync(eventData, result, ct);
    }

    private Task<List<IDomainEvent>> CollectAndClearDomainEventsAsync(DbContext? context)
    {
        if (context is null) return Task.FromResult<List<IDomainEvent>>([]);

        var aggregates = context.ChangeTracker.Entries<IAggregate>().Select(e => e.Entity).ToList();

        var domainEvents = aggregates.SelectMany(e => e.DomainEvents).ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        return Task.FromResult(domainEvents);
    }
}