using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Infrastructure.Repositories;

public class EnginesRepository(AppDbContext context) : IEnginesRepository
{
    public async Task<Result<Engine, Error>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var engine = await context.Engines.FindAsync(id, cancellationToken);
        if (engine == null)
        {
            return Result.Failure<Engine, Error>(Error.NotFound(
                nameof(Engine).ToLower(),
                $"Engine with id {id} not found."));
        }

        return Result.Success<Engine, Error>(engine);
    }

    public async Task<Result<int, Error>> AddAsync(Engine engine, CancellationToken cancellationToken)
    {
        await context.Engines.AddAsync(engine, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(engine.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(Engine engine, CancellationToken cancellationToken)
    {
        context.Engines.Update(engine);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(engine.Id);
    }

    public async Task<Result<int, Error>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var engineResult = await GetByIdAsync(id, cancellationToken);
        if (engineResult.IsFailure)
        {
            return Result.Failure<int, Error>(engineResult.Error);
        }

        var engine = engineResult.Value;
        context.Engines.Remove(engine);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(id);
    }
}