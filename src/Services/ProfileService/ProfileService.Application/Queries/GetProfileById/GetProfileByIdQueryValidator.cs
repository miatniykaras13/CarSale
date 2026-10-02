using FluentValidation;

namespace ProfileService.Application.Queries.GetProfileById;

public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
{
    public GetProfileByIdQueryValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty().WithMessage("Profile ID is required.");
    }
}
