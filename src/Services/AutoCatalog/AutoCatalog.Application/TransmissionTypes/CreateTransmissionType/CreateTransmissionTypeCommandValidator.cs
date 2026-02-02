namespace AutoCatalog.Application.TransmissionTypes.CreateTransmissionType;

public class CreateTransmissionTypeCommandValidator : AbstractValidator<CreateTransmissionTypeCommand>
{
    public CreateTransmissionTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
    }
}