using FluentValidation;
using ProfileService.Domain.Aggregates;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Application.Commands.UpdateMyProfile;

public class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandValidator()
    {
        RuleFor(x => x.KeycloakId)
            .NotEmpty().WithMessage("Keycloak ID must be a non-empty GUID.");

        RuleFor(x => x.Profile).NotNull().WithMessage("Profile is required.");

        When(x => x.Profile is not null, () =>
        {
            RuleFor(x => x.Profile.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(UserProfile.MAX_USERNAME_LENGTH);

            RuleFor(x => x.Profile.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .Must(value => Email.Of(value).IsSuccess).WithMessage("Email is invalid.");

            RuleFor(x => x.Profile.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(UserProfile.MAX_NAME_LENGTH);

            RuleFor(x => x.Profile.Surname)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Surname is required.")
                .MaximumLength(UserProfile.MAX_SURNAME_LENGTH);

            RuleFor(x => x.Profile.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"\A\+[1-9][0-9]{5,14}\z").WithMessage("Phone number must be in E.164 format.");

            RuleFor(x => x.Profile.Picture).MaximumLength(UserProfile.MAX_PICTURE_LENGTH);
        });
    }
}
