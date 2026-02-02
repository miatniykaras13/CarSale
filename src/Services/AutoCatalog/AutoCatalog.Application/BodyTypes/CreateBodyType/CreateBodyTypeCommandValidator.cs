namespace AutoCatalog.Application.BodyTypes.CreateBodyType;

public class CreateBodyTypeCommandValidator : AbstractValidator<CreateBodyTypeCommand>
{
    public CreateBodyTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
    }
}