namespace AutoCatalog.Application.Generations.CreateGeneration;

public class CreateGenerationCommandValidator : AbstractValidator<CreateGenerationCommand>
{
    public CreateGenerationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("Model Id is required");

        RuleFor(x => x.YearFrom)
            .GreaterThanOrEqualTo(1900).WithMessage("YearFrom must be greater than or equal to 1900")
            .LessThanOrEqualTo(x => x.YearTo).WithMessage("YearFrom must be less than or equal to the current year");

        RuleFor(x => x.YearTo)
            .GreaterThanOrEqualTo(x => x.YearFrom).WithMessage("YearTo must be greater than or equal to 1900")
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("YearTo must be less than or equal to the current year");
    }
}