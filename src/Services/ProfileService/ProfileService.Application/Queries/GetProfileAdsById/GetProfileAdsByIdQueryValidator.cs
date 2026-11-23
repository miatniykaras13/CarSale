using FluentValidation;

namespace ProfileService.Application.Queries.GetProfileAdsById;

public class GetProfileAdsByIdQueryValidator : AbstractValidator<GetProfileAdsByIdQuery>
{
    public GetProfileAdsByIdQueryValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty().WithMessage("Profile ID is required.");
    }
}
