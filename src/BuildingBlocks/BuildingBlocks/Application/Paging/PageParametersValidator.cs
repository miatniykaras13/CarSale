using FluentValidation;

namespace BuildingBlocks.Application.Paging;

public class PageParametersValidator : AbstractValidator<PageParameters>
{
    public PageParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page size must be greater than or equal to 0");
    }
}