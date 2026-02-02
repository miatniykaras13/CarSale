using AutoCatalog.Application.AutoDriveTypes.GetDriveTypeById;

namespace AutoCatalog.Application.BodyTypes.GetBodyTypeById;

public class GetBodyTypeByIdQueryValidator : AbstractValidator<GetDriveTypeByIdQuery>
{
    public GetBodyTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required").WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}