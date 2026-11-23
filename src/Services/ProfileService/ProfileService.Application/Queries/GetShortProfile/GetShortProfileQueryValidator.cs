using FluentValidation;

namespace ProfileService.Application.Queries.GetShortProfile;

public class GetShortProfileQueryValidator : AbstractValidator<GetShortProfileQuery>
{
    public GetShortProfileQueryValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty().WithMessage("Profile ID is required.");
    }
}
