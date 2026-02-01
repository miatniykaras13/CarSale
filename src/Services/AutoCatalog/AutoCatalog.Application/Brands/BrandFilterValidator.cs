namespace AutoCatalog.Application.Brands;

public class BrandFilterValidator : AbstractValidator<BrandFilter>
{
    public BrandFilterValidator()
    {
        When(x => x.YearTo is not null, () =>
        {
            RuleFor(x => x.YearFrom)
                .GreaterThanOrEqualTo(1900).WithMessage("YearFrom must be greater than or equal to 1900")
                .LessThanOrEqualTo(x => x.YearTo).WithMessage("YearFrom must be less than or equal to the YearTo");
        });

        When(x => x.YearTo is null, () =>
        {
            RuleFor(x => x.YearFrom)
                .GreaterThanOrEqualTo(1900).WithMessage("YearFrom must be greater than or equal to 1900")
                .LessThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("YearFrom must be less than or equal to the current year");
        });

        When(x => x.YearFrom is null, () =>
        {
            RuleFor(x => x.YearTo)
                .GreaterThanOrEqualTo(1900).WithMessage("YearTo must be greater than or equal to 1900")
                .LessThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("YearTo must be less than or equal to the current year");
        });

        When(x => x.YearFrom is not null, () =>
        {
            RuleFor(x => x.YearTo)
                .GreaterThanOrEqualTo(x => x.YearFrom).WithMessage("YearTo must be greater than or equal to YearFrom")
                .LessThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("YearTo must be less than or equal to the current year");
        });
    }
}