using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCarById;

public record GetCarByIdQuery(Guid Id) : IQuery<Result<Car, List<Error>>>;

public class GetCarByIdQueryHandler(ICarsRepository carsRepository)
    : IQueryHandler<GetCarByIdQuery, Result<Car, List<Error>>>
{
    public async Task<Result<Car, List<Error>>> Handle(GetCarByIdQuery query, CancellationToken cancellationToken)
    {
        var carResult = await carsRepository.GetByIdAsync(query.Id, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<Car, List<Error>>([carResult.Error]);

        return Result.Success<Car, List<Error>>(carResult.Value);
    }
}