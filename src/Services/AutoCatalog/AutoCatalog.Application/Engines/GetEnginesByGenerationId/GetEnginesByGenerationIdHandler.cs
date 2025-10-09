using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Engines.GetEnginesByGenerationId;

public record GetEnginesByGenerationIdQuery(int GenerationId) : IQuery<Result<List<Engine>, List<Error>>>;

public class GetEnginesByGenerationIdQueryHandler(
    IEnginesRepository enginesRepository,
    IGenerationsRepository generationsRepository)
    : IQueryHandler<GetEnginesByGenerationIdQuery, Result<List<Engine>, List<Error>>>
{
    public async Task<Result<List<Engine>, List<Error>>> Handle(
        GetEnginesByGenerationIdQuery query,
        CancellationToken cancellationToken)
    {
        var generationResult = await generationsRepository.GetByIdAsync(query.GenerationId, cancellationToken);

        if (generationResult.IsFailure)
            return Result.Failure<List<Engine>, List<Error>>([generationResult.Error]);

        var engineResult = await enginesRepository.GetByGenerationIdAsync(query.GenerationId, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<List<Engine>, List<Error>>([engineResult.Error]);

        return Result.Success<List<Engine>, List<Error>>(engineResult.Value);
    }
}