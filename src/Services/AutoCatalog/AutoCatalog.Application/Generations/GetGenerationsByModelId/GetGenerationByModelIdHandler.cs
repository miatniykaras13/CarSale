using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Generations.GetGenerationsByModelId;

public record GetGenerationsByModelIdQuery(int ModelId) : IQuery<Result<List<Generation>, List<Error>>>;

public class GetGenerationsByModelIdQueryHandler(
    IGenerationsRepository generationsRepository,
    IModelsRepository modelsRepository)
    : IQueryHandler<GetGenerationsByModelIdQuery, Result<List<Generation>, List<Error>>>
{
    public async Task<Result<List<Generation>, List<Error>>> Handle(
        GetGenerationsByModelIdQuery query,
        CancellationToken cancellationToken)
    {
        var modelResult = await modelsRepository.GetByIdAsync(query.ModelId, cancellationToken);

        if (modelResult.IsFailure)
            return Result.Failure<List<Generation>, List<Error>>([modelResult.Error]);

        var generationResult = await generationsRepository.GetByModelIdAsync(query.ModelId, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<List<Generation>, List<Error>>([generationResult.Error]);

        return Result.Success<List<Generation>, List<Error>>(generationResult.Value);
    }
}