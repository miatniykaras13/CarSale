using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.CQRS;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Brands.GetBrands;

public record GetBrandsQuery(BrandFilter Filter,
    SortParameters SortParameters,
    PageParameters PageParameters) : IQuery<Result<List<Brand>, List<Error>>>;

public class GetBrandsQueryHandler(
    IBrandsRepository brandsRepository,
    ILogger<GetBrandsQueryHandler> logger) : IQueryHandler<GetBrandsQuery, Result<List<Brand>, List<Error>>>
{
    public async Task<Result<List<Brand>, List<Error>>> Handle(GetBrandsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetBrandsQueryHandler.Handle called with {@Query}", query);

        var brandResult = await brandsRepository.GetAllAsync(query.Filter, query.SortParameters, query.PageParameters, cancellationToken);
        if (brandResult.IsFailure)
            return Result.Failure<List<Brand>, List<Error>>([brandResult.Error]);

        return Result.Success<List<Brand>, List<Error>>(brandResult.Value);
    }
}