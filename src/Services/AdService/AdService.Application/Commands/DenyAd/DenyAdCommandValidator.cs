using AdService.Application.Commands.CreateAd;
using AdService.Domain.ValueObjects;
using FluentValidation;

namespace AdService.Application.Commands.DenyAd;

public class DenyAdCommandValidator : AbstractValidator<DenyAdCommand>
{
    public DenyAdCommandValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");

        RuleFor(x => x.ModeratorId).NotEmpty().WithMessage("Moderator's id is required");

        RuleFor(x => x.ModerationResult.DenyReason).NotNull().WithMessage("Deny reason is required");

        RuleFor(x => x.ModerationResult.Message)
            .MaximumLength(ModerationResult.MAX_MESSAGE_LENGTH)
            .WithMessage($"Max message length is {ModerationResult.MAX_MESSAGE_LENGTH}");
    }
}