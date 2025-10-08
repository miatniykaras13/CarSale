namespace AutoCatalog.Application.Brands.DeleteBrand;

public class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}