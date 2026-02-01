namespace AutoCatalog.Application.AutoDriveTypes.CreateDriveType;

public class CreateDriveTypeCommandValidator : AbstractValidator<CreateDriveTypeCommand>
{
    public CreateDriveTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
    }
}