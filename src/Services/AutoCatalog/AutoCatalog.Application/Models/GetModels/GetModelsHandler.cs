using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Models.GetModels;

public record GetModelsQuery() : IQuery<Result<List<Model>, List<Error>>>;

public class GetModelsQueryHandler(IModelsRepository modelsRepository)
    : IQueryHandler<GetModelsQuery, Result<List<Model>, List<Error>>>
{
    public async Task<Result<List<Model>, List<Error>>> Handle(
        GetModelsQuery query,
        CancellationToken cancellationToken)
    {
        var modelResult = await modelsRepository.GetAllAsync(cancellationToken);
        if (modelResult.IsFailure)
            return Result.Failure<List<Model>, List<Error>>([modelResult.Error]);

        return Result.Success<List<Model>, List<Error>>(modelResult.Value);
    }
}