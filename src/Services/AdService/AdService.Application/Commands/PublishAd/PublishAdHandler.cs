using AdService.Application.Abstractions.Data;
using AdService.Common.Options;
using AdService.Domain.ValueObjects;
using Microsoft.Extensions.Options;

namespace AdService.Application.Commands.PublishAd;

public class PublishAdCommandHandler(
    IAppDbContext dbContext,
    IOptions<AdExpirationOptions> options) : ICommandHandler<PublishAdCommand, Result<PublishAdResponse, List<Error>>>
{
    private readonly AdExpirationOptions _expirationOptions = options.Value;

    private readonly string _defaultSuccessMessage = "Ad has been successfully moderated!";

    public async Task<Result<PublishAdResponse, List<Error>>> Handle(
        PublishAdCommand command,
        CancellationToken ct)
    {
        var moderatorId = command.ModeratorId;

        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

        if (ad is null)
        {
            return Result.Failure<PublishAdResponse, List<Error>>(Error.NotFound(
                "ad",
                $"Ad with id {command.AdId} not found"));
        }

        var moderationResult = ModerationResult.Of(
            moderatorId,
            decidedAt: DateTime.UtcNow,
            denyReason: null,
            message: _defaultSuccessMessage);

        if (moderationResult.IsFailure)
            return Result.Failure<PublishAdResponse, List<Error>>(moderationResult.Error);

        var publishResult = ad.Publish(_expirationOptions.AdLifeSpan, moderationResult.Value);

        if (publishResult.IsFailure)
            return Result.Failure<PublishAdResponse, List<Error>>(publishResult.Error);

        await dbContext.SaveChangesAsync(ct);

        return Result.Success<PublishAdResponse, List<Error>>(new PublishAdResponse(ad.ExpiresAt!.Value));
    }
}