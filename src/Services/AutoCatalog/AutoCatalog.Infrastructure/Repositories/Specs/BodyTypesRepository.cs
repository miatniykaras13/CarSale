using System.Linq.Expressions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.BodyTypes;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Specs;

public class BodyTypesRepository(AppDbContext context) : IBodyTypesRepository
{
    public async Task<Result<BodyType, Error>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var bodyType = await context.BodyTypes.FindAsync([id], cancellationToken);
        if (bodyType == null)
        {
            return Result.Failure<BodyType, Error>(Error.NotFound(
                nameof(BodyType).ToLower(),
                $"BodyType with id {id} not found."));
        }

        return Result.Success<BodyType, Error>(bodyType);
    }

    public async Task<Result<List<BodyType>, Error>> GetAllAsync(
        BodyTypeFilter bodyTypeFilter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var bodyTypes = await context.BodyTypes.AsNoTracking().ToListAsync(cancellationToken);
        return Result.Success<List<BodyType>, Error>(bodyTypes);
    }

    public async Task<Result<int, Error>> AddAsync(BodyType bodyType, CancellationToken cancellationToken = default)
    {
        await context.BodyTypes.AddAsync(bodyType, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(bodyType.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(BodyType bodyType, CancellationToken cancellationToken = default)
    {
        context.BodyTypes.Update(bodyType);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(bodyType.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var bodyTypeResult = await GetByIdAsync(id, cancellationToken);
        if (bodyTypeResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(bodyTypeResult.Error);
        }

        var bodyType = bodyTypeResult.Value;
        context.BodyTypes.Remove(bodyType);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }
}