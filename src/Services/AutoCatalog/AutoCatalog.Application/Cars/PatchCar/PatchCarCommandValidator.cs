namespace AutoCatalog.Application.Cars.PatchCar;

public class PatchCarCommandValidator : AbstractValidator<PatchCarCommand>
{
    public PatchCarCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}