using AdService.Contracts.Ads;
using AdService.Contracts.Ads.MergePatch;
using AdService.Domain.ValueObjects;
using FluentValidation;

namespace AdService.Application.Commands.MergePatchAdsCar;

public class MergePatchAdsCarCommandValidator : AbstractValidator<MergePatchAdsCarCommand>
{
    public MergePatchAdsCarCommandValidator()
    {
        RuleFor(x => x.AdId).NotEmpty().WithMessage("Ad's id is required");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required");
    }
}