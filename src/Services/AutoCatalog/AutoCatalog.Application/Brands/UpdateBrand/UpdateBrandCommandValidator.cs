namespace AutoCatalog.Application.Brands.UpdateBrand;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}