using AutoCatalog.Application.Abstractions;

namespace AutoCatalog.Application.Generations.PatchGeneration;

public record PatchGenerationCommand(int Id, string? Name)
    : ICommand<Result<int, List<Error>>>;

internal class PatchGenerationCommandHandler(IGenerationsRepository generationsRepository)
    : ICommandHandler<PatchGenerationCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(PatchGenerationCommand command, CancellationToken cancellationToken)
    {
        var generationResult = await generationsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<int, List<Error>>([generationResult.Error]);

        var generation = generationResult.Value;

        command.Adapt(generationResult.Value);

        await generationsRepository.AddAsync(generation, cancellationToken);

        return Result.Success<int, List<Error>>(generation.Id);
    }
}