using AutoCatalog.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Engines.DeleteEngine;

public record DeleteEngineCommand(int Id) : ICommand<Result<Unit, List<Error>>>;

public class DeleteEngineQueryHandler(
    IEnginesRepository enginesRepository,
    ILogger<DeleteEngineQueryHandler> logger) : ICommandHandler<DeleteEngineCommand, Result<Unit, List<Error>>>
{
    public async Task<Result<Unit, List<Error>>> Handle(DeleteEngineCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteEngineQueryHandler.Handle called with {@Command}", command);

        var engineResult = await enginesRepository.DeleteAsync(command.Id, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<Unit, List<Error>>([engineResult.Error]);

        return Result.Success<Unit, List<Error>>(Unit.Value);
    }
}