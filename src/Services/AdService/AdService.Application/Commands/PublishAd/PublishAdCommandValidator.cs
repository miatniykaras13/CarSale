using FluentValidation;

namespace AdService.Application.Commands.PublishAd;

public class PublishAdCommandValidator : AbstractValidator<PublishAdCommand>
{
    public PublishAdCommandValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
        RuleFor(x => x.ModeratorId).NotEmpty().WithMessage("Moderator's id is required");
    }
}