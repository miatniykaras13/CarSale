using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.AutoDriveTypes.GetDriveTypes;

public record GetDriveTypesQuery(
    AutoDriveTypeFilter Filter,
    SortParameters SortParameters,
    PageParameters PageParameters) : IQuery<Result<List<AutoDriveType>, List<Error>>>;

public class GetDriveTypesQueryHandler(
    IAutoDriveTypesRepository driveTypesRepository) : IQueryHandler<GetDriveTypesQuery, Result<List<AutoDriveType>, List<Error>>>
{
    public async Task<Result<List<AutoDriveType>, List<Error>>> Handle(
        GetDriveTypesQuery query,
        CancellationToken cancellationToken)
    {
        var driveTypeResult = await driveTypesRepository.GetAllAsync(
            query.Filter,
            query.SortParameters,
            query.PageParameters,
            cancellationToken);

        if (driveTypeResult.IsFailure)
            return Result.Failure<List<AutoDriveType>, List<Error>>([driveTypeResult.Error]);

        var driveTypes = driveTypeResult.Value;
        return Result.Success<List<AutoDriveType>, List<Error>>(driveTypes);
    }
}