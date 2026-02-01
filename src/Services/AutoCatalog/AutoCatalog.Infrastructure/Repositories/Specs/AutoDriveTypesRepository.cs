using System.Linq.Expressions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.AutoDriveTypes;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Specs;

public class AutoDriveTypesRepository(AppDbContext context) : IAutoDriveTypesRepository
{
    public async Task<Result<AutoDriveType, Error>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var driveType = await context.DriveTypes.FindAsync([id], cancellationToken);
        if (driveType == null)
        {
            return Result.Failure<AutoDriveType, Error>(Error.NotFound(
                nameof(AutoDriveType).ToLower(),
                $"AutoDriveType with id {id} not found."));
        }

        return Result.Success<AutoDriveType, Error>(driveType);
    }

    public async Task<Result<List<AutoDriveType>, Error>> GetAllAsync(
        AutoDriveTypeFilter driveTypeFilter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var driveTypes = await context.DriveTypes.AsNoTracking().ToListAsync(cancellationToken);
        return Result.Success<List<AutoDriveType>, Error>(driveTypes);
    }

    public async Task<Result<int, Error>> AddAsync(AutoDriveType driveType, CancellationToken cancellationToken = default)
    {
        await context.DriveTypes.AddAsync(driveType, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(driveType.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(AutoDriveType driveType, CancellationToken cancellationToken = default)
    {
        context.DriveTypes.Update(driveType);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(driveType.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var driveTypeResult = await GetByIdAsync(id, cancellationToken);
        if (driveTypeResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(driveTypeResult.Error);
        }

        var driveType = driveTypeResult.Value;
        context.DriveTypes.Remove(driveType);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }
}