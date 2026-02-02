using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.BodyTypes.GetBodyTypes;

public class GetBodyTypesQueryValidator : AbstractValidator<GetBodyTypesQuery>
{
    public GetBodyTypesQueryValidator()
    {
        RuleFor(x => x.PageParameters).SetValidator(new PageParametersValidator());
        RuleFor(x => x.SortParameters).SetValidator(new SortParametersValidator());
    }
}