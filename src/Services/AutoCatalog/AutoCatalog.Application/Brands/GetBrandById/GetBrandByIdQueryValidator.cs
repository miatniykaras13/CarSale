namespace AutoCatalog.Application.Brands.GetBrandById;

public class GetBrandByIdQueryValidator : AbstractValidator<GetBrandByIdQuery>
{
    public GetBrandByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}