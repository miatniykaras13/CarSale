using AutoCatalog.Application.Models.PatchModel;

namespace AutoCatalog.Application.Models.PatchModel;

public class PatchModelCommandValidator : AbstractValidator<PatchModelCommand>
{
    public PatchModelCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}