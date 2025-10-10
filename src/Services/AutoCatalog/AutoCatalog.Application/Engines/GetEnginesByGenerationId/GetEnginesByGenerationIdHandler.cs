using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Engines.GetEnginesByGenerationId;

public record GetEnginesByGenerationIdQuery(EngineFilter Filter, SortParameters SortParameters, PageParameters PageParameters, int GenerationId) : IQuery<Result<List<Engine>, List<Error>>>;

public class GetEnginesByGenerationIdQueryHandler(
    IEnginesRepository enginesRepository,
    IGenerationsRepository generationsRepository,
    ILogger<GetEnginesByGenerationIdQueryHandler> logger)
    : IQueryHandler<GetEnginesByGenerationIdQuery, Result<List<Engine>, List<Error>>>
{
    public async Task<Result<List<Engine>, List<Error>>> Handle(
        GetEnginesByGenerationIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetEnginesByGenerationIdQueryHandler.Handle called with {@Query}", query);

        var generationResult = await generationsRepository.GetByIdAsync(query.GenerationId, cancellationToken);

        if (generationResult.IsFailure)
            return Result.Failure<List<Engine>, List<Error>>([generationResult.Error]);

        var engineResult = await enginesRepository.GetByGenerationIdAsync(query.Filter, query.SortParameters, query.PageParameters, query.GenerationId, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<List<Engine>, List<Error>>([engineResult.Error]);

        return Result.Success<List<Engine>, List<Error>>(engineResult.Value);
    }
}