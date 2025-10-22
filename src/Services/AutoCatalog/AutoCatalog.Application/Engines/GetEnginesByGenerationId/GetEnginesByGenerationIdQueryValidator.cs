using AutoCatalog.Application.Engines.GetEngineById;

namespace AutoCatalog.Application.Engines.GetEnginesByGenerationId;

public class GetEnginesByGenerationIdQueryValidator : AbstractValidator<GetEnginesByGenerationIdQuery>
{
    public GetEnginesByGenerationIdQueryValidator()
    {
        RuleFor(x => x.GenerationId)
            .NotEmpty().WithMessage("Generation Id is required")
            .GreaterThan(0).WithMessage("Generation Id must be greater than 0");

        RuleFor(x => x.Filter).SetValidator(new EngineFilterValidator());
    }
}