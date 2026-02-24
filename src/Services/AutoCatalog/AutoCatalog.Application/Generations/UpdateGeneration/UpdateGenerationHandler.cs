using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;

namespace AutoCatalog.Application.Generations.UpdateGeneration;

public record UpdateGenerationCommand(int Id, string Name, int ModelId, int YearFrom, int? YearTo)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateGenerationCommandHandler(
    IGenerationsRepository generationsRepository,
    IPublishEndpoint publishEndpoint)
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

        if (generation.ModelId != command.ModelId)
        {
            return Result.Failure<int, List<Error>>(Error.Validation(
                "model_id",
                "Cannot change model id. Recreate the generation"));
        }

        if (!generation.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
        {
            var engineUpdatedEvent = new GenerationUpdatedEvent
            {
                GenerationId = command.Id,
                GenerationName = command.Name,
                YearFrom = command.YearFrom,
                YearTo = command.YearTo ?? DateTime.UtcNow.Year,
                ModelId = command.ModelId,
            };
            await publishEndpoint.Publish(engineUpdatedEvent, cancellationToken);
        }

        command.Adapt(generation);

        await generationsRepository.AddAsync(generation, cancellationToken);

        return Result.Success<int, List<Error>>(generation.Id);
    }
}