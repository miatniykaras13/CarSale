namespace AutoCatalog.Application.Cars.GetCarsByBrandId;

public class GetCarsByBrandIdQueryValidator : AbstractValidator<GetCarsByBrandIdQuery>
{
    public GetCarsByBrandIdQueryValidator()
    {
        RuleFor(x => x.BrandId).NotEmpty().WithMessage("Brand Id is required");

        RuleFor(x => x.Filter).SetValidator(new CarFilterValidator());
    }
}