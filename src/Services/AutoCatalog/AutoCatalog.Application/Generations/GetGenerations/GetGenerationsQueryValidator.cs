using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Generations.GetGenerations;

public class GetGenerationsQueryValidator : AbstractValidator<GetGenerationsQuery>
{
    public GetGenerationsQueryValidator()
    {
        RuleFor(x => x.Filter).SetValidator(new GenerationFilterValidator());
        RuleFor(x => x.PageParameters).SetValidator(new PageParametersValidator());
        RuleFor(x => x.SortParameters).SetValidator(new SortParametersValidator());
    }
}