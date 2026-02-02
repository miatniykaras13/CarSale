using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;

namespace AutoCatalog.Application.BodyTypes.GetBodyTypeById;

public record GetBodyTypeByIdQuery(int Id) : IQuery<Result<BodyType, List<Error>>>;

public class GetBodyTypeByIdQueryHandler(IBodyTypesRepository bodyTypesRepository)
    : IQueryHandler<GetBodyTypeByIdQuery, Result<BodyType, List<Error>>>
{
    public async Task<Result<BodyType, List<Error>>> Handle(
        GetBodyTypeByIdQuery query,
        CancellationToken cancellationToken)
    {
        var bodyTypeResult = await bodyTypesRepository.GetByIdAsync(query.Id, cancellationToken);
        if (bodyTypeResult.IsFailure)
            return Result.Failure<BodyType, List<Error>>([bodyTypeResult.Error]);

        var bodyType = bodyTypeResult.Value;
        return Result.Success<BodyType, List<Error>>(bodyType);
    }
}