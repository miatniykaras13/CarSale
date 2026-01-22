using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.DenyAd;

public class DenyAdCommandHandler(IAppDbContext dbContext) : ICommandHandler<DenyAdCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(DenyAdCommand command, CancellationToken ct)
    {
        if (command.ModerationResult.DenyReason is null)
            return UnitResult.Failure<List<Error>>(Error.Validation("moderation_result", "Deny reason is required"));

        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

        if (ad is null)
            return UnitResult.Failure<List<Error>>(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        var moderationResult = ModerationResult.Of(
            command.ModeratorId,
            DateTime.UtcNow,
            command.ModerationResult.DenyReason,
            command.ModerationResult.Message);

        if (moderationResult.IsFailure)
            return UnitResult.Failure<List<Error>>(moderationResult.Error);

        var denyResult = ad.Deny(moderationResult.Value);

        if (denyResult.IsFailure) return UnitResult.Failure<List<Error>>(denyResult.Error);

        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<List<Error>>();
    }
}