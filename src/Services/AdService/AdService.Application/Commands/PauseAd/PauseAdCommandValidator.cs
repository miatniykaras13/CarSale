using FluentValidation;

namespace AdService.Application.Commands.PauseAd;

public class PauseAdCommandValidator : AbstractValidator<PauseAdCommand>
{
    public PauseAdCommandValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
    }
}