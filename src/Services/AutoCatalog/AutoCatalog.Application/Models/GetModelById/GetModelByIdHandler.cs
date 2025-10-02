using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Models.GetModelById;

public record GetModelByIdQuery(int Id) : IQuery<Result<Model, List<Error>>>;

public class GetModelByIdQueryHandler(
    IModelsRepository modelsRepository,
    ILogger<GetModelByIdQueryHandler> logger) : IQueryHandler<GetModelByIdQuery, Result<Model, List<Error>>>
{
    public async Task<Result<Model, List<Error>>> Handle(GetModelByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetModelByIdQueryHandler.Handle called with {@Query}", query);

        var modelResult = await modelsRepository.GetByIdAsync(query.Id, cancellationToken);
        if (modelResult.IsFailure)
            return Result.Failure<Model, List<Error>>([modelResult.Error]);

        return Result.Success<Model, List<Error>>(modelResult.Value);
    }
}