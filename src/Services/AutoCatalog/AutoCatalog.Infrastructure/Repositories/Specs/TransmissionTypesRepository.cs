using System.Linq.Expressions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.TransmissionTypes;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Specs;

public class TransmissionTypesRepository(AppDbContext context) : ITransmissionTypesRepository
{
    public async Task<Result<TransmissionType, Error>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var transmissionType = await context.TransmissionTypes.FindAsync([id], cancellationToken);
        if (transmissionType == null)
        {
            return Result.Failure<TransmissionType, Error>(Error.NotFound(
                nameof(TransmissionType).ToLower(),
                $"TransmissionType with id {id} not found."));
        }

        return Result.Success<TransmissionType, Error>(transmissionType);
    }

    public async Task<Result<List<TransmissionType>, Error>> GetAllAsync(
        TransmissionTypeFilter transmissionTypeFilter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var transmissionTypes = await context.TransmissionTypes.AsNoTracking().ToListAsync(cancellationToken);
        return Result.Success<List<TransmissionType>, Error>(transmissionTypes);
    }

    public async Task<Result<int, Error>> AddAsync(
        TransmissionType transmissionType,
        CancellationToken cancellationToken = default)
    {
        await context.TransmissionTypes.AddAsync(transmissionType, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(transmissionType.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(
        TransmissionType transmissionType,
        CancellationToken cancellationToken = default)
    {
        context.TransmissionTypes.Update(transmissionType);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(transmissionType.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var transmissionTypeResult = await GetByIdAsync(id, cancellationToken);
        if (transmissionTypeResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(transmissionTypeResult.Error);
        }

        var transmissionType = transmissionTypeResult.Value;
        context.TransmissionTypes.Remove(transmissionType);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }
}