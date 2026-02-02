using AutoCatalog.Application.AutoDriveTypes.UpdateDriveType;

namespace AutoCatalog.Application.TransmissionTypes.UpdateTransmissionType;

public class UpdateTransmissionTypeCommandValidator : AbstractValidator<UpdateDriveTypeCommand>
{
    public UpdateTransmissionTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}