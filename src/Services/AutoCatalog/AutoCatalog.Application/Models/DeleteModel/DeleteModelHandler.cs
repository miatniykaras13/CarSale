using AutoCatalog.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Models.DeleteModel;

public record DeleteModelCommand(int Id) : ICommand<Result<Unit, List<Error>>>;

public class DeleteModelQueryHandler(
    IModelsRepository modelsRepository,
    ILogger<DeleteModelQueryHandler> logger) : ICommandHandler<DeleteModelCommand, Result<Unit, List<Error>>>
{
    public async Task<Result<Unit, List<Error>>> Handle(DeleteModelCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteModelQueryHandler.Handle called with {@Command}", command);

        var modelResult = await modelsRepository.DeleteAsync(command.Id, cancellationToken);
        if (modelResult.IsFailure)
            return Result.Failure<Unit, List<Error>>([modelResult.Error]);

        return Result.Success<Unit, List<Error>>(Unit.Value);
    }
}