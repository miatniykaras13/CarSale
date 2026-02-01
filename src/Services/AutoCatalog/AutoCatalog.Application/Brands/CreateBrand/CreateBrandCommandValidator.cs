namespace AutoCatalog.Application.Brands.CreateBrand;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required");

        RuleFor(x => x.YearFrom)
            .GreaterThan(1900).WithMessage("YearFrom must be greater than 1900")
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("YearTo must be less than or equal to current year");

        When(x => x.YearTo is not null, () =>
        {
            RuleFor(x => x.YearTo)
                .GreaterThan(x => x.YearFrom).WithMessage("YearTo must be greater than YearFrom")
                .LessThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("YearTo must be less than or equal to current year");

            RuleFor(x => x.YearFrom)
                .GreaterThan(1900).WithMessage("YearFrom must be greater than 1900")
                .LessThanOrEqualTo(x => x.YearTo)
                .WithMessage("YearFrom must be less than or equal to YearTo");
        });
    }
}