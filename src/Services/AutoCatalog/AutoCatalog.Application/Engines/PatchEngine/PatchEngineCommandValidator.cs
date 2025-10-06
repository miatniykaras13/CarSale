using AutoCatalog.Application.Engines.PatchEngine;

namespace AutoCatalog.Application.Engines.PatchEngine;

public class PatchEngineCommandValidator : AbstractValidator<PatchEngineCommand>
{
    public PatchEngineCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}