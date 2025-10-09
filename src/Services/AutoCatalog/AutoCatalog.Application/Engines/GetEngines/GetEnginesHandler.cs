using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Engines.GetEngines;

public record GetEnginesQuery() : IQuery<Result<List<Engine>, List<Error>>>;

public class GetEnginesQueryHandler(IEnginesRepository enginesRepository)
    : IQueryHandler<GetEnginesQuery, Result<List<Engine>, List<Error>>>
{
    public async Task<Result<List<Engine>, List<Error>>> Handle(
        GetEnginesQuery query,
        CancellationToken cancellationToken)
    {
        var engineResult = await enginesRepository.GetAllAsync(cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<List<Engine>, List<Error>>([engineResult.Error]);

        return Result.Success<List<Engine>, List<Error>>(engineResult.Value);
    }
}