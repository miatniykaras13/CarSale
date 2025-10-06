namespace AutoCatalog.Application.Brands.PatchBrand;

public class PatchBrandCommandValidator : AbstractValidator<PatchBrandCommand>
{
    public PatchBrandCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}