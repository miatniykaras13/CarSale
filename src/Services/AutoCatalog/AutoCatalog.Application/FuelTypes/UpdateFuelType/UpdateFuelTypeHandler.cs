using AutoCatalog.Application.Abstractions.Repositories;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;

namespace AutoCatalog.Application.FuelTypes.UpdateFuelType;

public record UpdateFuelTypeCommand(int Id, string Name)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateFuelTypeCommandHandler(
    IFuelTypesRepository fuelTypesRepository,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateFuelTypeCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(
        UpdateFuelTypeCommand command,
        CancellationToken cancellationToken)
    {
        var fuelTypeResult = await fuelTypesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (fuelTypeResult.IsFailure)
            return Result.Failure<int, List<Error>>([fuelTypeResult.Error]);

        var fuelType = fuelTypeResult.Value;

        if (!fuelType.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
        {
            var bodyTypeUpdatedEvent = new FuelTypeUpdatedEvent
            {
                FuelTypeId = command.Id, FuelTypeName = command.Name,
            };
            await publishEndpoint.Publish(bodyTypeUpdatedEvent, cancellationToken);
        }

        command.Adapt(fuelType);

        await fuelTypesRepository.UpdateAsync(fuelType, cancellationToken);

        return Result.Success<int, List<Error>>(fuelType.Id);
    }
}