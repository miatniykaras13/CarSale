using System.Linq.Expressions;
using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Generations;
using AutoCatalog.Application.Generations.Extensions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Specs;

public class GenerationsRepository(AppDbContext context) : IGenerationsRepository
{
    public async Task<Result<Generation, Error>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var generation = await context.Generations.FindAsync([id], cancellationToken);
        if (generation == null)
        {
            return Result.Failure<Generation, Error>(Error.NotFound(
                nameof(Generation).ToLower(),
                $"Generation with id {id} not found."));
        }

        return Result.Success<Generation, Error>(generation);
    }

    public async Task<Result<List<Generation>, Error>> GetAllAsync(
        GenerationFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var generations = await context.Generations
            .AsNoTracking()
            .Filter(filter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .ToListAsync(cancellationToken);
        return Result.Success<List<Generation>, Error>(generations);
    }

    public async Task<Result<int, Error>> AddAsync(Generation generation, CancellationToken cancellationToken = default)
    {
        await context.Generations.AddAsync(generation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(generation.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(Generation generation,
        CancellationToken cancellationToken = default)
    {
        context.Generations.Update(generation);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(generation.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var generationResult = await GetByIdAsync(id, cancellationToken);
        if (generationResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(generationResult.Error);
        }

        var generation = generationResult.Value;
        context.Generations.Remove(generation);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }
}