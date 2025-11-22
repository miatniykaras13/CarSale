namespace AutoCatalog.Application.Brands;

public class BrandFilterValidator : AbstractValidator<BrandFilter>
{
    public BrandFilterValidator()
    {
        RuleFor(x => x.YearFrom)
            .GreaterThanOrEqualTo(1900).WithMessage("YearFrom must be greater than or equal to 1900")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("YearFrom must be less than or equal to the current year");

        RuleFor(x => x.YearTo)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("YearTo must be less than or equal to the current year")
            .GreaterThanOrEqualTo(x => x.YearFrom).WithMessage("YearTo must be greater than or equal to YearFrom");
    }
}