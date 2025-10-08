using AutoCatalog.Application.Generations.DeleteGeneration;

namespace AutoCatalog.Application.Generations.DeleteGeneration;

public class DeleteGenerationCommandValidator : AbstractValidator<DeleteGenerationCommand>
{
    public DeleteGenerationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}