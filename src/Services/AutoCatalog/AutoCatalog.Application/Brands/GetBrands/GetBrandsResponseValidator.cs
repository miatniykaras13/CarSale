namespace AutoCatalog.Application.Brands.GetBrands;

public class GetBrandsResponseValidator : AbstractValidator<GetBrandsQuery>
{
    public GetBrandsResponseValidator()
    {
        RuleFor(x => x.Filter).SetValidator(new BrandFilterValidator());
    }
}