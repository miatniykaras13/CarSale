using AutoCatalog.Application.Abstractions.Repositories;

namespace AutoCatalog.Application.AutoDriveTypes.DeleteDriveType;

public record DeleteDriveTypeCommand(int Id) : ICommand<UnitResult<List<Error>>>;

public class DeleteDriveTypeQueryHandler(IAutoDriveTypesRepository driveTypesRepository)
    : ICommandHandler<DeleteDriveTypeCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(DeleteDriveTypeCommand command, CancellationToken cancellationToken)
    {
        var driveTypeUnitResult = await driveTypesRepository.DeleteAsync(command.Id, cancellationToken);
        if (driveTypeUnitResult.IsFailure)
            return UnitResult.Failure<List<Error>>([driveTypeUnitResult.Error]);

        return UnitResult.Success<List<Error>>();
    }
}