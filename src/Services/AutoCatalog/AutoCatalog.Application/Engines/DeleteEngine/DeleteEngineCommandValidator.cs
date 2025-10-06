using AutoCatalog.Application.Engines.DeleteEngine;

namespace AutoCatalog.Application.Engines.DeleteEngine;

public class DeleteEngineCommandValidator : AbstractValidator<DeleteEngineCommand>
{
    public DeleteEngineCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}