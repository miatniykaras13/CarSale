using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Generations.GetGenerationsByModelId;

public record GetGenerationsByModelIdQuery(GenerationFilter Filter, SortParameters SortParameters, PageParameters PageParameters, int ModelId) : IQuery<Result<List<Generation>, List<Error>>>;

public class GetGenerationsByModelIdQueryHandler(
    IGenerationsRepository generationsRepository,
    IModelsRepository modelsRepository,
    ILogger<GetGenerationsByModelIdQueryHandler> logger)
    : IQueryHandler<GetGenerationsByModelIdQuery, Result<List<Generation>, List<Error>>>
{
    public async Task<Result<List<Generation>, List<Error>>> Handle(
        GetGenerationsByModelIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetGenerationsByModelIdQueryHandler.Handle called with {@Query}", query);

        var modelResult = await modelsRepository.GetByIdAsync(query.ModelId, cancellationToken);

        if (modelResult.IsFailure)
            return Result.Failure<List<Generation>, List<Error>>([modelResult.Error]);

        var generationResult = await generationsRepository.GetByModelIdAsync(query.Filter, query.SortParameters, query.PageParameters, query.ModelId, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<List<Generation>, List<Error>>([generationResult.Error]);

        return Result.Success<List<Generation>, List<Error>>(generationResult.Value);
    }
}