namespace AutoCatalog.Application.TransmissionTypes.DeleteTransmissionType;

public class DeleteTransmissionTypeCommandValidator : AbstractValidator<DeleteTransmissionTypeCommand>
{
    public DeleteTransmissionTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}