using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Generations.GetGenerations;

public record GetGenerationsQuery(GenerationFilter Filter, SortParameters SortParameters, PageParameters PageParameters) : IQuery<Result<List<Generation>, List<Error>>>;

public class GetGenerationsQueryHandler(
    IGenerationsRepository generationsRepository) : IQueryHandler<GetGenerationsQuery, Result<List<Generation>, List<Error>>>
{
    public async Task<Result<List<Generation>, List<Error>>> Handle(GetGenerationsQuery query, CancellationToken cancellationToken)
    {
        var generationResult = await generationsRepository.GetAllAsync(query.Filter, query.SortParameters, query.PageParameters, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<List<Generation>, List<Error>>([generationResult.Error]);

        return Result.Success<List<Generation>, List<Error>>(generationResult.Value);
    }
}