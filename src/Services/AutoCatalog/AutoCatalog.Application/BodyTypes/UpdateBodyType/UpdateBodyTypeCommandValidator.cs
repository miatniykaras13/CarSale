using AutoCatalog.Application.AutoDriveTypes.UpdateDriveType;

namespace AutoCatalog.Application.BodyTypes.UpdateBodyType;

public class UpdateBodyTypeCommandValidator : AbstractValidator<UpdateDriveTypeCommand>
{
    public UpdateBodyTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}