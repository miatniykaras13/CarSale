using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Models.GetModels;

public record GetModelsQuery(ModelFilter Filter, SortParameters SortParameters, PageParameters PageParameters) : IQuery<Result<List<Model>, List<Error>>>;

public class GetModelsQueryHandler(
    IModelsRepository modelsRepository) : IQueryHandler<GetModelsQuery, Result<List<Model>, List<Error>>>
{
    public async Task<Result<List<Model>, List<Error>>> Handle(GetModelsQuery query, CancellationToken cancellationToken)
    {
        var modelResult = await modelsRepository.GetAllAsync(query.Filter, query.SortParameters, query.PageParameters, cancellationToken);
        if (modelResult.IsFailure)
            return Result.Failure<List<Model>, List<Error>>([modelResult.Error]);

        return Result.Success<List<Model>, List<Error>>(modelResult.Value);
    }
}