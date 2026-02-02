using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Brands.GetBrands;

public record GetBrandsQuery(
    BrandFilter Filter,
    SortParameters SortParameters,
    PageParameters PageParameters) : IQuery<Result<List<Brand>, List<Error>>>;

public class GetBrandsQueryHandler(
    IBrandsRepository brandsRepository) : IQueryHandler<GetBrandsQuery, Result<List<Brand>, List<Error>>>
{
    public async Task<Result<List<Brand>, List<Error>>> Handle(
        GetBrandsQuery query,
        CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.GetAllAsync(
            query.Filter,
            query.SortParameters,
            query.PageParameters,
            cancellationToken);

        if (brandResult.IsFailure)
            return Result.Failure<List<Brand>, List<Error>>([brandResult.Error]);

        var brands = brandResult.Value;

        brands.ForEach(b => b.YearTo ??= DateTime.UtcNow.Year);
        return Result.Success<List<Brand>, List<Error>>(brands);
    }
}