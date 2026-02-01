namespace AutoCatalog.Application.Generations.UpdateGeneration;

public class UpdateGenerationCommandValidator : AbstractValidator<UpdateGenerationCommand>
{
    public UpdateGenerationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
        
        RuleFor(x => x.YearFrom)
            .GreaterThanOrEqualTo(1900).WithMessage("YearFrom must be greater than or equal to 1900")
            .LessThanOrEqualTo(x => x.YearTo).WithMessage("YearFrom must be less than or equal to the current year");

        RuleFor(x => x.YearTo)
            .GreaterThanOrEqualTo(x => x.YearFrom).WithMessage("YearTo must be greater than or equal to 1900")
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("YearTo must be less than or equal to the current year");
    }
}