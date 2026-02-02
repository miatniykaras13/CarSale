using System.Linq.Expressions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.FuelTypes;
using AutoCatalog.Application.FuelTypes.Extensions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Specs;

public class FuelTypesRepository(AppDbContext context) : IFuelTypesRepository
{
    public async Task<Result<FuelType, Error>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var fuelType = await context.FuelTypes.FindAsync([id], cancellationToken);
        if (fuelType == null)
        {
            return Result.Failure<FuelType, Error>(Error.NotFound(
                nameof(FuelType).ToLower(),
                $"FuelType with id {id} not found."));
        }

        return Result.Success<FuelType, Error>(fuelType);
    }

    public async Task<Result<List<FuelType>, Error>> GetAllAsync(
        FuelTypeFilter fuelTypeFilter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var fuelTypes = await context.FuelTypes
            .AsNoTracking()
            .Filter(fuelTypeFilter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .ToListAsync(cancellationToken);
        return Result.Success<List<FuelType>, Error>(fuelTypes);
    }

    public async Task<Result<int, Error>> AddAsync(FuelType fuelType, CancellationToken cancellationToken = default)
    {
        await context.FuelTypes.AddAsync(fuelType, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(fuelType.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(FuelType fuelType, CancellationToken cancellationToken = default)
    {
        context.FuelTypes.Update(fuelType);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(fuelType.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var fuelTypeResult = await GetByIdAsync(id, cancellationToken);
        if (fuelTypeResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(fuelTypeResult.Error);
        }

        var fuelType = fuelTypeResult.Value;
        context.FuelTypes.Remove(fuelType);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }
}