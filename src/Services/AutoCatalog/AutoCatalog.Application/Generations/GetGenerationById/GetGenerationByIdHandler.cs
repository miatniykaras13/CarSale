using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Generations.GetGenerationById;

public record GetGenerationByIdQuery(int Id) : IQuery<Result<Generation, List<Error>>>;

public class GetGenerationByIdQueryHandler(
    IGenerationsRepository generationsRepository) : IQueryHandler<GetGenerationByIdQuery, Result<Generation, List<Error>>>
{
    public async Task<Result<Generation, List<Error>>> Handle(GetGenerationByIdQuery query, CancellationToken cancellationToken)
    {
        var generationResult = await generationsRepository.GetByIdAsync(query.Id, cancellationToken);
        if (generationResult.IsFailure)
            return Result.Failure<Generation, List<Error>>([generationResult.Error]);

        return Result.Success<Generation, List<Error>>(generationResult.Value);
    }
}