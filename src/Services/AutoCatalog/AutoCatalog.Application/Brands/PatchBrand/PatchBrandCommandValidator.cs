namespace AutoCatalog.Application.Brands.PatchBrand;

public class PatchBrandCommandValidator : AbstractValidator<PatchBrandCommand>
{
    public PatchBrandCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}