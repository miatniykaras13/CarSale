using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Brands.GetBrandById;

public record GetBrandByIdQuery(int Id) : IQuery<Result<Brand, List<Error>>>;

public class GetBrandByIdQueryHandler(IBrandsRepository brandsRepository)
    : IQueryHandler<GetBrandByIdQuery, Result<Brand, List<Error>>>
{
    public async Task<Result<Brand, List<Error>>> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.GetByIdAsync(query.Id, cancellationToken);
        if (brandResult.IsFailure)
            return Result.Failure<Brand, List<Error>>([brandResult.Error]);

        var brand = brandResult.Value;
        if (brand.IsActive)
            brand.YearTo = DateTime.UtcNow.Year;

        return Result.Success<Brand, List<Error>>(brand);
    }
}