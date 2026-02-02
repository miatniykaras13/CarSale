using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Engines.GetEngines;

public class GetEnginesQueryValidator : AbstractValidator<GetEnginesQuery>
{
    public GetEnginesQueryValidator()
    {
        RuleFor(x => x.Filter).SetValidator(new EngineFilterValidator());
        RuleFor(x => x.PageParameters).SetValidator(new PageParametersValidator());
        RuleFor(x => x.SortParameters).SetValidator(new SortParametersValidator());
    }
}