using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Brands.GetBrands;

public class GetBrandsQueryValidator : AbstractValidator<GetBrandsQuery>
{
    public GetBrandsQueryValidator()
    {
        RuleFor(x => x.Filter).SetValidator(new BrandFilterValidator());
        RuleFor(x => x.PageParameters).SetValidator(new PageParametersValidator());
        RuleFor(x => x.SortParameters).SetValidator(new SortParametersValidator());
    }
}