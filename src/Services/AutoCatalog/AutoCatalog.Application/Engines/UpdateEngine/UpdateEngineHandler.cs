using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

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

internal class UpdateEngineCommandHandler(IEnginesRepository enginesRepository)
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

        command.Adapt(engine);

        await enginesRepository.UpdateAsync(engine, cancellationToken);

        return Result.Success<int, List<Error>>(engine.Id);
    }
}