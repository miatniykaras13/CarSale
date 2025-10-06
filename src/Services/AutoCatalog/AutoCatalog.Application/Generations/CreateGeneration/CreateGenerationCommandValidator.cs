namespace AutoCatalog.Application.Generations.CreateGeneration;

public class CreateGenerationCommandValidator : AbstractValidator<CreateGenerationCommand>
{
    public CreateGenerationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("Model Id is required");
    }
}