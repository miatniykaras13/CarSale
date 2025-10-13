using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCars;

public record GetCarsQuery(CarFilter Filter, SortParameters SortParameters, PageParameters PageParameters) : IQuery<Result<List<Car>, List<Error>>>;

public class GetCarsQueryHandler(
    ICarsRepository carsRepository) : IQueryHandler<GetCarsQuery, Result<List<Car>, List<Error>>>
{
    public async Task<Result<List<Car>, List<Error>>> Handle(GetCarsQuery query, CancellationToken cancellationToken)
    {
        var carResult = await carsRepository.GetAllAsync(query.Filter, query.SortParameters, query.PageParameters, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([carResult.Error]);

        return Result.Success<List<Car>, List<Error>>(carResult.Value);
    }
}