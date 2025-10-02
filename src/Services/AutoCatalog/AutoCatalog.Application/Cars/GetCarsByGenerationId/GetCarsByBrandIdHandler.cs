using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Transport.Cars;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Cars.GetCarsByGenerationId;

public record GetCarsByGenerationIdQuery(int GenerationId) : IQuery<Result<List<Car>, List<Error>>>;

public class GetCarsByGenerationIdQueryHandler(
    ICarsRepository carsRepository,
    IGenerationsRepository generationsRepository,
    ILogger<GetCarsByGenerationIdQueryHandler> logger)
    : IQueryHandler<GetCarsByGenerationIdQuery, Result<List<Car>, List<Error>>>
{
    public async Task<Result<List<Car>, List<Error>>> Handle(
        GetCarsByGenerationIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetCarsByGenerationIdQueryHandler.Handle called with {@Query}", query);

        var generationResult = await generationsRepository.GetByIdAsync(query.GenerationId, cancellationToken);

        if (generationResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([generationResult.Error]);

        var carResult = await carsRepository.GetByGenerationIdAsync(query.GenerationId, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<List<Car>, List<Error>>([carResult.Error]);

        return Result.Success<List<Car>, List<Error>>(carResult.Value);
    }
}