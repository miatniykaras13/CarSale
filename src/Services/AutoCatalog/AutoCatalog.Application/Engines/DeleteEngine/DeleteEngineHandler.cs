using AutoCatalog.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Engines.DeleteEngine;

public record DeleteEngineCommand(int Id) : ICommand<Result<Unit, List<Error>>>;

public class DeleteEngineQueryHandler(
    IEnginesRepository enginesRepository) : ICommandHandler<DeleteEngineCommand, Result<Unit, List<Error>>>
{
    public async Task<Result<Unit, List<Error>>> Handle(DeleteEngineCommand command, CancellationToken cancellationToken)
    {
        var engineResult = await enginesRepository.DeleteAsync(command.Id, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<Unit, List<Error>>([engineResult.Error]);

        return Result.Success<Unit, List<Error>>(Unit.Value);
    }
}