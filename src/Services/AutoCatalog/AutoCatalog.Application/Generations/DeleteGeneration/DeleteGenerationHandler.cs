using AutoCatalog.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Generations.DeleteGeneration;

public record DeleteGenerationCommand(int Id) : ICommand<Result<Unit, List<Error>>>;

public class DeleteGenerationQueryHandler(
    IGenerationsRepository generationsRepository) : ICommandHandler<DeleteGenerationCommand, Result<Unit, List<Error>>>
{
    public async Task<Result<Unit, List<Error>>> Handle(DeleteGenerationCommand command, CancellationToken cancellationToken)
    {

        var generationResult = await generationsRepository.DeleteAsync(command.Id, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<Unit, List<Error>>([generationResult.Error]);

        return Result.Success<Unit, List<Error>>(Unit.Value);
    }
}