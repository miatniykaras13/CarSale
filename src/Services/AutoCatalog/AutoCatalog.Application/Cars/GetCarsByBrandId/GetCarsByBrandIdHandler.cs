using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCarsByBrandId;

public record GetCarsByBrandIdQuery(int BrandId) : IQuery<Result<List<Car>, List<Error>>>;

public class GetCarsByBrandIdQueryHandler(
    ICarsRepository carsRepository,
    IBrandsRepository brandsRepository,
    ILogger<GetCarsByBrandIdQueryHandler> logger)
    : IQueryHandler<GetCarsByBrandIdQuery, Result<List<Car>, List<Error>>>
{
    public async Task<Result<List<Car>, List<Error>>> Handle(
        GetCarsByBrandIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetCarsByBrandIdQueryHandler.Handle called with {@Query}", query);

        var brandResult = await brandsRepository.GetByIdAsync(query.BrandId, cancellationToken);

        if (brandResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([brandResult.Error]);

        var carResult = await carsRepository.GetByBrandIdAsync(query.BrandId, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([carResult.Error]);

        return Result.Success<List<Car>, List<Error>>(carResult.Value);
    }
}