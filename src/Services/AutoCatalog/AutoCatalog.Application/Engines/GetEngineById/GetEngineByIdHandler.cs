using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Engines.GetEngineById;

public record GetEngineByIdQuery(int Id) : IQuery<Result<Engine, List<Error>>>;

public class GetEngineByIdQueryHandler(
    IEnginesRepository enginesRepository,
    ILogger<GetEngineByIdQueryHandler> logger) : IQueryHandler<GetEngineByIdQuery, Result<Engine, List<Error>>>
{
    public async Task<Result<Engine, List<Error>>> Handle(GetEngineByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetEngineByIdQueryHandler.Handle called with {@Query}", query);

        var engineResult = await enginesRepository.GetByIdAsync(query.Id, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<Engine, List<Error>>([engineResult.Error]);

        return Result.Success<Engine, List<Error>>(engineResult.Value);
    }
}