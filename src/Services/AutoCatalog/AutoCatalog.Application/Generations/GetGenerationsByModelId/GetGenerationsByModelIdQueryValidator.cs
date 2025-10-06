using AutoCatalog.Application.Generations.GetGenerationById;

namespace AutoCatalog.Application.Generations.GetGenerationsByModelId;

public class GetGenerationsByModelIdQueryValidator : AbstractValidator<GetGenerationsByModelIdQuery>
{
    public GetGenerationsByModelIdQueryValidator()
    {
        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("Model Id is required");
    }
}