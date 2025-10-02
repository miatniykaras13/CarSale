using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.CQRS;

namespace AutoCatalog.Application.Engines.CreateEngine;

public record CreateEngineCommand(
    FuelType FuelType,
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
            FuelType = command.FuelType,
            GenerationId = command.GenerationId,
            Name = command.Name,
            Volume = command.Volume,
            HorsePower = command.HorsePower,
            TorqueNm = command.TorqueNm,
        };

        await enginesRepository.AddAsync(engine, cancellationToken);

        return Result.Success<int, List<Error>>(engine.Id);
    }
}