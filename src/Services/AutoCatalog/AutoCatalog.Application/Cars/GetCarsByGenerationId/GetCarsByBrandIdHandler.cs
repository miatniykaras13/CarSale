using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCarsByGenerationId;

public record GetCarsByGenerationIdQuery(CarFilter Filter, SortParameters SortParameters, PageParameters PageParameters, int GenerationId) : IQuery<Result<List<Car>, List<Error>>>;

public class GetCarsByGenerationIdQueryHandler(
    ICarsRepository carsRepository,
    IGenerationsRepository generationsRepository)
    : IQueryHandler<GetCarsByGenerationIdQuery, Result<List<Car>, List<Error>>>
{
    public async Task<Result<List<Car>, List<Error>>> Handle(
        GetCarsByGenerationIdQuery query,
        CancellationToken cancellationToken)
    {
        var generationResult = await generationsRepository.GetByIdAsync(query.GenerationId, cancellationToken);

        if (generationResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([generationResult.Error]);

        var carResult = await carsRepository.GetByGenerationIdAsync(query.Filter, query.SortParameters, query.PageParameters, query.GenerationId, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([carResult.Error]);

        return Result.Success<List<Car>, List<Error>>(carResult.Value);
    }
}