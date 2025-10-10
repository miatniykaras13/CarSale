using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Engines.PatchEngine;

public record PatchEngineCommand(int Id, string? Name)
    : ICommand<Result<int, List<Error>>>;

internal class PatchEngineCommandHandler(IEnginesRepository enginesRepository)
    : ICommandHandler<PatchEngineCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(PatchEngineCommand command, CancellationToken cancellationToken)
    {
        TypeAdapterConfig<PatchEngineCommand, Engine>
            .NewConfig()
            .IgnoreNullValues(true);

        var engineResult = await enginesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<int, List<Error>>([engineResult.Error]);

        var engine = engineResult.Value;

        command.Adapt(engine);

        await enginesRepository.AddAsync(engine, cancellationToken);

        return Result.Success<int, List<Error>>(engine.Id);
    }
}