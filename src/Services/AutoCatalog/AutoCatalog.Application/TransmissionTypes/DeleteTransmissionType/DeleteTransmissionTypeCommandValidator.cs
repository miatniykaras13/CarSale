using AutoCatalog.Application.TransmissionTypes.DeleteTransmissionType;

namespace AutoCatalog.Application.BodyTypes.DeleteBodyType;

public class DeleteTransmissionTypeCommandValidator : AbstractValidator<DeleteTransmissionTypeCommand>
{
    public DeleteTransmissionTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}