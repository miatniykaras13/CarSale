using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCarsByModelId;

public record GetCarsByModelIdQuery(CarFilter Filter, SortParameters SortParameters, PageParameters PageParameters, int ModelId) : IQuery<Result<List<Car>, List<Error>>>;

public class GetCarsByModelIdQueryHandler(
    ICarsRepository carsRepository,
    IModelsRepository modelsRepository)
    : IQueryHandler<GetCarsByModelIdQuery, Result<List<Car>, List<Error>>>
{
    public async Task<Result<List<Car>, List<Error>>> Handle(
        GetCarsByModelIdQuery query,
        CancellationToken cancellationToken)
    {
        var modelResult = await modelsRepository.GetByIdAsync(query.ModelId, cancellationToken);

        if (modelResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([modelResult.Error]);

        var carResult = await carsRepository.GetByModelIdAsync(query.Filter, query.SortParameters, query.PageParameters, query.ModelId, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([carResult.Error]);

        return Result.Success<List<Car>, List<Error>>(carResult.Value);
    }
}