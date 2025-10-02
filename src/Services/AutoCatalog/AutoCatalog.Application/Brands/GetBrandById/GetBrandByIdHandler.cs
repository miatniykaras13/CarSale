using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Brands.GetBrandById;

public record GetBrandByIdQuery(int Id) : IQuery<Result<Brand, List<Error>>>;

public class GetBrandByIdQueryHandler(
    IBrandsRepository brandsRepository,
    ILogger<GetBrandByIdQueryHandler> logger) : IQueryHandler<GetBrandByIdQuery, Result<Brand, List<Error>>>
{
    public async Task<Result<Brand, List<Error>>> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetBrandByIdQueryHandler.Handle called with {@Query}", query);

        var brandResult = await brandsRepository.GetByIdAsync(query.Id, cancellationToken);
        if (brandResult.IsFailure)
            return Result.Failure<Brand, List<Error>>([brandResult.Error]);

        return Result.Success<Brand, List<Error>>(brandResult.Value);
    }
}