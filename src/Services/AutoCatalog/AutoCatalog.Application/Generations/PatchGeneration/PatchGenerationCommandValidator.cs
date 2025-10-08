using AutoCatalog.Application.Generations.PatchGeneration;

namespace AutoCatalog.Application.Generations.PatchGeneration;

public class PatchGenerationCommandValidator : AbstractValidator<PatchGenerationCommand>
{
    public PatchGenerationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}