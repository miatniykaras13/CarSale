using System.Linq.Expressions;
using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Engines;
using AutoCatalog.Application.Engines.Extensions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Specs;

public class EnginesRepository(AppDbContext context) : IEnginesRepository
{
    public async Task<Result<Engine, Error>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var engine = await context.Engines.FindAsync([id], cancellationToken);
        if (engine == null)
        {
            return Result.Failure<Engine, Error>(Error.NotFound(
                nameof(Engine).ToLower(),
                $"Engine with id {id} not found."));
        }

        return Result.Success<Engine, Error>(engine);
    }

    public async Task<Result<List<Engine>, Error>> GetAllAsync(
        EngineFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var engines = await context.Engines
            .AsNoTracking()
            .Include(e => e.FuelType)
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters).ToListAsync(cancellationToken);
        return Result.Success<List<Engine>, Error>(engines);
    }

    public async Task<Result<int, Error>> AddAsync(Engine engine, CancellationToken cancellationToken = default)
    {
        await context.Engines.AddAsync(engine, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(engine.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(Engine engine, CancellationToken cancellationToken = default)
    {
        context.Engines.Update(engine);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(engine.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var engineResult = await GetByIdAsync(id, cancellationToken);
        if (engineResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(engineResult.Error);
        }

        var engine = engineResult.Value;
        context.Engines.Remove(engine);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public async Task<Result<List<Engine>, Error>> GetByGenerationIdAsync(
        EngineFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        int generationId,
        CancellationToken cancellationToken = default,
        params Expression<Func<Engine, object>>[] includes)
    {
        IQueryable<Engine> query = context.Engines.Where(e => e.GenerationId == generationId);

        foreach (var include in includes)
            query = query.Include(include);

        query = query
            .AsNoTracking()
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters);

        var engines = await query.ToListAsync(cancellationToken);

        return Result.Success<List<Engine>, Error>(engines);
    }
}