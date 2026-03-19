using BuildingBlocks.Application.Paging;
using FluentValidation;

namespace AdService.Application.Queries.GetAllCarOptions;

public class GetAllCarOptionsQueryValidator : AbstractValidator<GetAllCarOptionsQuery>
{
    public GetAllCarOptionsQueryValidator()
    {
        RuleFor(x => x.PageParameters).SetValidator(new PageParametersValidator());
    }
}

