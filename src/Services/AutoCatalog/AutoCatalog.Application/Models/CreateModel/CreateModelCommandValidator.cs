namespace AutoCatalog.Application.Models.CreateModel;

public class CreateModelCommandValidator : AbstractValidator<CreateModelCommand>
{
    public CreateModelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("Brand Id is required");
    }
}