using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Engines.GetEngines;

public record GetEnginesQuery(EngineFilter Filter, SortParameters SortParameters, PageParameters PageParameters) : IQuery<Result<List<Engine>, List<Error>>>;

public class GetEnginesQueryHandler(
    IEnginesRepository enginesRepository,
    ILogger<GetEnginesQueryHandler> logger) : IQueryHandler<GetEnginesQuery, Result<List<Engine>, List<Error>>>
{
    public async Task<Result<List<Engine>, List<Error>>> Handle(GetEnginesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetEnginesQueryHandler.Handle called with {@Query}", query);

        var engineResult = await enginesRepository.GetAllAsync(query.Filter, query.SortParameters, query.PageParameters, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<List<Engine>, List<Error>>([engineResult.Error]);

        return Result.Success<List<Engine>, List<Error>>(engineResult.Value);
    }
}