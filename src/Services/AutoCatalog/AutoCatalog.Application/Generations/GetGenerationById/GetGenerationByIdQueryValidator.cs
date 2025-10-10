namespace AutoCatalog.Application.Generations.GetGenerationById;

public class GetGenerationByIdQueryValidator : AbstractValidator<GetGenerationByIdQuery>
{
    public GetGenerationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}