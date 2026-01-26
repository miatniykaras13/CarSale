using AdService.Application.Commands.PublishAd;
using FluentValidation;

namespace AdService.Application.Commands.SubmitAd;

public class SubmitAdCommandValidator : AbstractValidator<SubmitAdCommand>
{
    public SubmitAdCommandValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
    }
}