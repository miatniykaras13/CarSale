using AutoCatalog.Application.Abstractions.Repositories;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;

namespace AutoCatalog.Application.AutoDriveTypes.UpdateDriveType;

public record UpdateDriveTypeCommand(int Id, string Name)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateDriveTypeCommandHandler(
    IAutoDriveTypesRepository driveTypesRepository,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateDriveTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(
        UpdateDriveTypeCommand command,
        CancellationToken cancellationToken)
    {
        var driveTypeResult = await driveTypesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (driveTypeResult.IsFailure)
            return Result.Failure<int, List<Error>>([driveTypeResult.Error]);

        var driveType = driveTypeResult.Value;

        if (!driveType.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
        {
            var bodyTypeUpdatedEvent = new AutoDriveTypeUpdatedEvent
            {
                AutoDriveTypeId = command.Id,
                AutoDriveTypeName = command.Name,
            };
            await publishEndpoint.Publish(bodyTypeUpdatedEvent, cancellationToken);
        }

        command.Adapt(driveType);

        await driveTypesRepository.UpdateAsync(driveType, cancellationToken);

        return Result.Success<int, List<Error>>(driveType.Id);
    }
}