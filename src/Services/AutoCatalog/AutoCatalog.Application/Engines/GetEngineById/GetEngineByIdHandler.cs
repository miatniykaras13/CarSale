using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.Engines.GetEngineById;

public record GetEngineByIdQuery(int Id) : IQuery<Result<Engine, List<Error>>>;

public class GetEngineByIdQueryHandler(IEnginesRepository enginesRepository)
    : IQueryHandler<GetEngineByIdQuery, Result<Engine, List<Error>>>
{
    public async Task<Result<Engine, List<Error>>> Handle(GetEngineByIdQuery query, CancellationToken cancellationToken)
    {
        var engineResult = await enginesRepository.GetByIdAsync(query.Id, cancellationToken);
        if (engineResult.IsFailure)
            return Result.Failure<Engine, List<Error>>([engineResult.Error]);

        return Result.Success<Engine, List<Error>>(engineResult.Value);
    }
}