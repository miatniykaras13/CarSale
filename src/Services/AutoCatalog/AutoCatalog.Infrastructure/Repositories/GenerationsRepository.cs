using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore;

namespace AutoCatalog.Infrastructure.Repositories;

public class GenerationsRepository(AppDbContext context) : IGenerationsRepository
{
    public async Task<Result<Generation, Error>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var generation = await context.Generations.FindAsync(id, cancellationToken);
        if (generation == null)
        {
            return Result.Failure<Generation, Error>(Error.NotFound(
                nameof(Generation).ToLower(),
                $"Generation with id {id} not found."));
        }

        return Result.Success<Generation, Error>(generation);
    }

    public async Task<Result<List<Generation>, Error>> GetAllAsync(CancellationToken cancellationToken)
    {
        var generations = await context.Generations.ToListAsync(cancellationToken);
        return Result.Success<List<Generation>, Error>(generations);
    }

    public async Task<Result<int, Error>> AddAsync(Generation generation, CancellationToken cancellationToken)
    {
        await context.Generations.AddAsync(generation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(generation.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(Generation generation, CancellationToken cancellationToken)
    {
        context.Generations.Update(generation);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(generation.Id);
    }

    public async Task<Result<int, Error>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var generationResult = await GetByIdAsync(id, cancellationToken);
        if (generationResult.IsFailure)
        {
            return Result.Failure<int, Error>(generationResult.Error);
        }

        var generation = generationResult.Value;
        context.Generations.Remove(generation);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(id);
    }
}