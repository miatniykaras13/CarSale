using AutoCatalog.Application.Engines.GetEnginesByGenerationId;

namespace AutoCatalog.Application.Engines.GetEngines;

public class GetEnginesQueryValidator : AbstractValidator<GetEnginesByGenerationIdQuery>
{
    public GetEnginesQueryValidator()
    {
        RuleFor(x => x.GenerationId)
            .NotEmpty().WithMessage("Generation Id is required")
            .GreaterThan(0).WithMessage("Generation Id must be greater than 0");

        RuleFor(x => x.Filter).SetValidator(new EngineFilterValidator());
    }
}