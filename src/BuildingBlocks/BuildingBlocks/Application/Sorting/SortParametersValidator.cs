using FluentValidation;

namespace BuildingBlocks.Application.Sorting;

public class SortParametersValidator : AbstractValidator<SortParameters>
{
    public SortParametersValidator()
    {
        RuleFor(x => x.Direction).IsInEnum().WithMessage("Direction is invalid");
    }
}