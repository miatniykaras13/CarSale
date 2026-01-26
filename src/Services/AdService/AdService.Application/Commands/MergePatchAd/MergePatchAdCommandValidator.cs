using AdService.Application.Commands.CreateAd;
using FluentValidation;

namespace AdService.Application.Commands.MergePatchAd;

public class MergePatchAdCommandValidator : AbstractValidator<MergePatchAdCommand>
{
    public MergePatchAdCommandValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
    }
}