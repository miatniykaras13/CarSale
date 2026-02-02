using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.BodyTypes.GetBodyTypes;

public record GetBodyTypesQuery(
    BodyTypeFilter Filter,
    SortParameters SortParameters,
    PageParameters PageParameters) : IQuery<Result<List<BodyType>, List<Error>>>;

public class GetBodyTypesQueryHandler(
    IBodyTypesRepository bodyTypesRepository) : IQueryHandler<GetBodyTypesQuery, Result<List<BodyType>, List<Error>>>
{
    public async Task<Result<List<BodyType>, List<Error>>> Handle(
        GetBodyTypesQuery query,
        CancellationToken cancellationToken)
    {
        var bodyTypeResult = await bodyTypesRepository.GetAllAsync(
            query.Filter,
            query.SortParameters,
            query.PageParameters,
            cancellationToken);

        if (bodyTypeResult.IsFailure)
            return Result.Failure<List<BodyType>, List<Error>>([bodyTypeResult.Error]);

        var bodyTypes = bodyTypeResult.Value;
        return Result.Success<List<BodyType>, List<Error>>(bodyTypes);
    }
}