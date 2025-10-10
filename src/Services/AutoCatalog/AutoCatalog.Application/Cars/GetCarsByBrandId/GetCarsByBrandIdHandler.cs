using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCarsByBrandId;

public record GetCarsByBrandIdQuery(CarFilter Filter, SortParameters SortParameters, PageParameters PageParameters, int BrandId) : IQuery<Result<List<Car>, List<Error>>>;

public class GetCarsByBrandIdQueryHandler(
    ICarsRepository carsRepository,
    IBrandsRepository brandsRepository)
    : IQueryHandler<GetCarsByBrandIdQuery, Result<List<Car>, List<Error>>>
{
    public async Task<Result<List<Car>, List<Error>>> Handle(
        GetCarsByBrandIdQuery query,
        CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.GetByIdAsync(query.BrandId, cancellationToken);

        if (brandResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([brandResult.Error]);

        var carResult = await carsRepository.GetByBrandIdAsync(query.Filter, query.SortParameters, query.PageParameters, query.BrandId, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([carResult.Error]);

        return Result.Success<List<Car>, List<Error>>(carResult.Value);
    }
}