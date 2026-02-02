using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Generations.UpdateGeneration;

public record UpdateGenerationCommand(int Id, string Name, int YearFrom, int? YearTo)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateGenerationCommandHandler(IGenerationsRepository generationsRepository)
    : ICommandHandler<UpdateGenerationCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(
        UpdateGenerationCommand command,
        CancellationToken cancellationToken)
    {
        var generationResult = await generationsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<int, List<Error>>([generationResult.Error]);

        var generation = generationResult.Value;

        command.Adapt(generation);

        await generationsRepository.AddAsync(generation, cancellationToken);

        return Result.Success<int, List<Error>>(generation.Id);
    }
}