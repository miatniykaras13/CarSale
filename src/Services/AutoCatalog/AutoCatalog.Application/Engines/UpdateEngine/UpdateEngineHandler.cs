using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;

namespace AutoCatalog.Application.Engines.UpdateEngine;

public record UpdateEngineCommand(
    int Id,
    int GenerationId,
    string Name,
    int FuelTypeId,
    float Volume,
    int HorsePower,
    int TorqueNm)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateEngineCommandHandler(
    IEnginesRepository enginesRepository,
    IFuelTypesRepository fuelTypesRepository,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateEngineCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(UpdateEngineCommand command, CancellationToken cancellationToken)
    {
        var engineResult = await enginesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<int, List<Error>>([engineResult.Error]);

        var engine = engineResult.Value;

        if (engine.GenerationId != command.GenerationId)
        {
            return Result.Failure<int, List<Error>>(Error.Validation(
                "generation_id",
                "Cannot update the generation id. Recreate the engine."));
        }

        var fuelType = engine.FuelType;

        if (engine.FuelTypeId != command.FuelTypeId)
        {
            var fuelTypeResult = await fuelTypesRepository.GetByIdAsync(command.FuelTypeId, cancellationToken);
            if (fuelTypeResult.IsFailure)
                return Result.Failure<int, List<Error>>(fuelTypeResult.Error);
            fuelType = fuelTypeResult.Value;
        }


        if (!engine.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase) ||
            engine.FuelTypeId != command.FuelTypeId ||
            engine.Volume.Equals(command.Volume) ||
            engine.HorsePower != command.HorsePower ||
            engine.TorqueNm != command.TorqueNm)
        {
            var engineUpdatedEvent = new EngineUpdatedEvent
            {
                EngineId = command.Id,
                EngineName = command.Name,
                FuelTypeId = fuelType.Id,
                FuelTypeName = fuelType.Name,
                Volume = command.Volume,
                HorsePower = command.HorsePower,
                GenerationId = command.GenerationId,
                TorqueNm = command.TorqueNm,
            };
            await publishEndpoint.Publish(engineUpdatedEvent, cancellationToken);
        }

        command.Adapt(engine);

        await enginesRepository.UpdateAsync(engine, cancellationToken);

        return Result.Success<int, List<Error>>(engine.Id);
    }
}