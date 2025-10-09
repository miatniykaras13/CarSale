using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCars;

public record GetCarsQuery() : IQuery<Result<List<Car>, List<Error>>>;

public class GetCarsQueryHandler(ICarsRepository carsRepository)
    : IQueryHandler<GetCarsQuery, Result<List<Car>, List<Error>>>
{
    public async Task<Result<List<Car>, List<Error>>> Handle(GetCarsQuery query, CancellationToken cancellationToken)
    {
        var carResult = await carsRepository.GetAllAsync(cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([carResult.Error]);

        return Result.Success<List<Car>, List<Error>>(carResult.Value);
    }
}