namespace AutoCatalog.Application.Cars.GetCarsByEngineId;

public class GetCarsByEngineIdQueryValidator : AbstractValidator<GetCarsByEngineIdQuery>
{
    public GetCarsByEngineIdQueryValidator()
    {
        RuleFor(x => x.EngineId).NotEmpty().WithMessage("Engine Id is required");
    }
}