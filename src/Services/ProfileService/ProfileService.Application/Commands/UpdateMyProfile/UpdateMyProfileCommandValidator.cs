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

        RuleFor(x => x.Profile.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(UserProfile.MAX_USERNAME_LENGTH);

        RuleFor(x => x.Profile.Email)
            .NotEmpty().WithMessage("Email is required.");

        RuleFor(x => x.Profile.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(UserProfile.MAX_NAME_LENGTH);

        RuleFor(x => x.Profile.Surname)
            .NotEmpty().WithMessage("Surname is required.")
            .MaximumLength(UserProfile.MAX_SURNAME_LENGTH);

        RuleFor(x => x.Profile.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.");
    }
}