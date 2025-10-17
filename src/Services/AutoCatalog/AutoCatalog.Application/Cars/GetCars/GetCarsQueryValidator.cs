namespace AutoCatalog.Application.Cars.GetCars;

public class GetCarsQueryValidator : AbstractValidator<GetCarsQuery>
{
    public GetCarsQueryValidator()
    {
        RuleFor(x => x.Filter).SetValidator(new CarFilterValidator());
    }
}