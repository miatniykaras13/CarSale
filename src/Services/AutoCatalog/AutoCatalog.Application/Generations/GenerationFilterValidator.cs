namespace AutoCatalog.Application.Generations;

public class GenerationFilterValidator : AbstractValidator<GenerationFilter>
{
    public GenerationFilterValidator()
    {
        RuleFor(x => x.ModelId)
            .GreaterThan(0).WithMessage("Model id must be greater than zero");

        RuleFor(x => x.YearFrom)
            .GreaterThanOrEqualTo(1900).WithMessage("YearFrom must be greater than or equal to 1900")
            .LessThanOrEqualTo(x => x.YearTo).WithMessage("YearFrom must be less than or equal to the current year");

        RuleFor(x => x.YearTo)
            .GreaterThanOrEqualTo(x => x.YearFrom).WithMessage("YearTo must be greater than or equal to 1900")
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("YearTo must be less than or equal to the current year");
    }
}