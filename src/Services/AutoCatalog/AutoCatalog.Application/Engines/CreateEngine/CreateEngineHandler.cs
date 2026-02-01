using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Engines.CreateEngine;

public record CreateEngineCommand(
    int FuelTypeId,
    int GenerationId,
    string Name,
    float Volume,
    int HorsePower,
    int TorqueNm)
    : ICommand<Result<int, List<Error>>>;

internal class CreateEngineCommandHandler(
    IEnginesRepository enginesRepository,
    IGenerationsRepository generationsRepository)
    : ICommandHandler<CreateEngineCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateEngineCommand command, CancellationToken cancellationToken)
    {
        var generationResult = await generationsRepository.GetByIdAsync(command.GenerationId, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<int, List<Error>>([generationResult.Error]);

        Engine engine = new()
        {
            FuelTypeId = command.FuelTypeId,
            GenerationId = command.GenerationId,
            Generation = generationResult.Value,
            Name = command.Name,
            Volume = command.Volume,
            HorsePower = command.HorsePower,
            TorqueNm = command.TorqueNm,
        };

        await enginesRepository.AddAsync(engine, cancellationToken);

        return Result.Success<int, List<Error>>(engine.Id);
    }
}