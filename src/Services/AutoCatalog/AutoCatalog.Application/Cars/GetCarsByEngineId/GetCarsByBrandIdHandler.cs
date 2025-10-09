using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Transport.Cars;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCarsByEngineId;

public record GetCarsByEngineIdQuery(int EngineId) : IQuery<Result<List<Car>, List<Error>>>;

public class GetCarsByEngineIdQueryHandler(
    ICarsRepository carsRepository,
    IEnginesRepository enginesRepository)
    : IQueryHandler<GetCarsByEngineIdQuery, Result<List<Car>, List<Error>>>
{
    public async Task<Result<List<Car>, List<Error>>> Handle(
        GetCarsByEngineIdQuery query,
        CancellationToken cancellationToken)
    {
        var engineResult = await enginesRepository.GetByIdAsync(query.EngineId, cancellationToken);

        if (engineResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([engineResult.Error]);

        var carResult = await carsRepository.GetByEngineIdAsync(query.EngineId, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([carResult.Error]);

        return Result.Success<List<Car>, List<Error>>(carResult.Value);
    }
}