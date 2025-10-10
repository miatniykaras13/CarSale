using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Models.GetModelsByBrandId;

public record GetModelsByBrandIdQuery(ModelFilter Filter, SortParameters SortParameters, PageParameters PageParameters, int BrandId) : IQuery<Result<List<Model>, List<Error>>>;

public class GetModelsByBrandIdQueryHandler(
    IModelsRepository modelsRepository,
    IBrandsRepository brandsRepository,
    ILogger<GetModelsByBrandIdQueryHandler> logger)
    : IQueryHandler<GetModelsByBrandIdQuery, Result<List<Model>, List<Error>>>
{
    public async Task<Result<List<Model>, List<Error>>> Handle(
        GetModelsByBrandIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetModelsByBrandIdQueryHandler.Handle called with {@Query}", query);

        var brandResult = await brandsRepository.GetByIdAsync(query.BrandId, cancellationToken);

        if (brandResult.IsFailure)
            return Result.Failure<List<Model>, List<Error>>([brandResult.Error]);

        var modelResult = await modelsRepository.GetByBrandIdAsync(query.Filter, query.SortParameters, query.PageParameters, query.BrandId, cancellationToken);
        if (modelResult.IsFailure)
            return Result.Failure<List<Model>, List<Error>>([modelResult.Error]);

        return Result.Success<List<Model>, List<Error>>(modelResult.Value);
    }
}