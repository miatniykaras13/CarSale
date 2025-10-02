using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.CQRS;

namespace AutoCatalog.Application.Generations.CreateGeneration;

public record CreateGenerationCommand(int ModelId, string Name)
    : ICommand<Result<int, List<Error>>>;

internal class CreateGenerationCommandHandler(
    IGenerationsRepository generationsRepository,
    IModelsRepository modelsRepository)
    : ICommandHandler<CreateGenerationCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateGenerationCommand command, CancellationToken cancellationToken)
    {
        var modelResult = await modelsRepository.GetByIdAsync(command.ModelId, cancellationToken);
        if (modelResult.IsFailure)
            return Result.Failure<int, List<Error>>([modelResult.Error]);
        
        Generation generation = new()
        {
            ModelId = command.ModelId,
            Name = command.Name,
        };

        await generationsRepository.AddAsync(generation, cancellationToken);

        return Result.Success<int, List<Error>>(generation.Id);
    }
}