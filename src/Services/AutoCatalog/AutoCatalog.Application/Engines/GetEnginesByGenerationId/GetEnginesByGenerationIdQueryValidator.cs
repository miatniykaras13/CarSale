using AutoCatalog.Application.Engines.GetEngineById;

namespace AutoCatalog.Application.Engines.GetEnginesByGenerationId;

public class GetEnginesByGenerationIdQueryValidator : AbstractValidator<GetEnginesByGenerationIdQuery>
{
    public GetEnginesByGenerationIdQueryValidator()
    {
        RuleFor(x => x.GenerationId)
            .NotEmpty().WithMessage("Generation Id is required");
    }
}