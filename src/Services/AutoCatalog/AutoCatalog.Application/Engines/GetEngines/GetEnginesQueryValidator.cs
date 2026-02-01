namespace AutoCatalog.Application.Engines.GetEngines;

public class GetEnginesQueryValidator : AbstractValidator<GetEnginesQuery>
{
    public GetEnginesQueryValidator()
    {
        RuleFor(x => x.Filter).SetValidator(new EngineFilterValidator());
    }
}