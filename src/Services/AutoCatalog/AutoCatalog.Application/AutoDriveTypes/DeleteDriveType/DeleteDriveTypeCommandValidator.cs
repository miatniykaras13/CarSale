namespace AutoCatalog.Application.AutoDriveTypes.DeleteDriveType;

public class DeleteDriveTypeCommandValidator : AbstractValidator<DeleteDriveTypeCommand>
{
    public DeleteDriveTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}