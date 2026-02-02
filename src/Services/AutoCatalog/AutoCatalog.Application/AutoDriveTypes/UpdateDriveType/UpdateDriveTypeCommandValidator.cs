namespace AutoCatalog.Application.AutoDriveTypes.UpdateDriveType;

public class UpdateDriveTypeCommandValidator : AbstractValidator<UpdateDriveTypeCommand>
{
    public UpdateDriveTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}