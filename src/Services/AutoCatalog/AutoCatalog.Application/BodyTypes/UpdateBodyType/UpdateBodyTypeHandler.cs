using AutoCatalog.Application.Abstractions.Repositories;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;

namespace AutoCatalog.Application.BodyTypes.UpdateBodyType;

public record UpdateBodyTypeCommand(int Id, string Name)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateBodyTypeCommandHandler(
    IBodyTypesRepository bodyTypesRepository,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateBodyTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(
        UpdateBodyTypeCommand command,
        CancellationToken cancellationToken)
    {
        var bodyTypeResult = await bodyTypesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (bodyTypeResult.IsFailure)
            return Result.Failure<int, List<Error>>([bodyTypeResult.Error]);

        var bodyType = bodyTypeResult.Value;

        if (!bodyType.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
        {
            var bodyTypeUpdatedEvent = new BodyTypeUpdatedEvent { BodyTypeId = command.Id, BodyTypeName = command.Name, };
            await publishEndpoint.Publish(bodyTypeUpdatedEvent, cancellationToken);
        }

        command.Adapt(bodyType);

        await bodyTypesRepository.UpdateAsync(bodyType, cancellationToken);

        return Result.Success<int, List<Error>>(bodyType.Id);
    }
}