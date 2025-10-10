using AutoCatalog.Application.Generations.GetGenerationById;

namespace AutoCatalog.Application.Generations.GetGenerationsByModelId;

public class GetGenerationsByModelIdQueryValidator : AbstractValidator<GetGenerationsByModelIdQuery>
{
    public GetGenerationsByModelIdQueryValidator()
    {
        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("Model Id is required")
            .GreaterThan(0).WithMessage("Model Id must be greater than 0");
    }
}