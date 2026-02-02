using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.AutoDriveTypes.CreateDriveType;

public record CreateDriveTypeCommand(string Name)
    : ICommand<Result<int, List<Error>>>;

internal class CreateDriveTypeCommandHandler(IAutoDriveTypesRepository driveTypesRepository)
    : ICommandHandler<CreateDriveTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateDriveTypeCommand command, CancellationToken cancellationToken)
    {
        AutoDriveType driveType = new()
        {
            Name = command.Name,
        };

        await driveTypesRepository.AddAsync(driveType, cancellationToken);

        return Result.Success<int, List<Error>>(driveType.Id);
    }
}

