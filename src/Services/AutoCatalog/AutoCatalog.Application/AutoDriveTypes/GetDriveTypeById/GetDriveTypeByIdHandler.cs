using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.AutoDriveTypes.GetDriveTypeById;

public record GetDriveTypeByIdQuery(int Id) : IQuery<Result<AutoDriveType, List<Error>>>;

public class GetDriveTypeByIdQueryHandler(IAutoDriveTypesRepository driveTypesRepository)
    : IQueryHandler<GetDriveTypeByIdQuery, Result<AutoDriveType, List<Error>>>
{
    public async Task<Result<AutoDriveType, List<Error>>> Handle(
        GetDriveTypeByIdQuery query,
        CancellationToken cancellationToken)
    {
        var driveTypeResult = await driveTypesRepository.GetByIdAsync(query.Id, cancellationToken);
        if (driveTypeResult.IsFailure)
            return Result.Failure<AutoDriveType, List<Error>>([driveTypeResult.Error]);

        var driveType = driveTypeResult.Value;
        return Result.Success<AutoDriveType, List<Error>>(driveType);
    }
}