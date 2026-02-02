namespace AutoCatalog.Application.BodyTypes.DeleteBodyType;

public class DeleteBodyTypeCommandValidator : AbstractValidator<DeleteBodyTypeCommand>
{
    public DeleteBodyTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}