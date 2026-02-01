namespace AutoCatalog.Application.AutoDriveTypes.GetDriveTypeById;

public class GetDriveTypeByIdQueryValidator : AbstractValidator<GetDriveTypeByIdQuery>
{
    public GetDriveTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}