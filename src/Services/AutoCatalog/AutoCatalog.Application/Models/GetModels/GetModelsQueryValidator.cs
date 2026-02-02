using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Models.GetModels;

public class GetModelsQueryValidator : AbstractValidator<GetModelsQuery>
{
    public GetModelsQueryValidator()
    {
        RuleFor(x => x.Filter).SetValidator(new ModelFilterValidator());
        RuleFor(x => x.PageParameters).SetValidator(new PageParametersValidator());
        RuleFor(x => x.SortParameters).SetValidator(new SortParametersValidator());
    }
}