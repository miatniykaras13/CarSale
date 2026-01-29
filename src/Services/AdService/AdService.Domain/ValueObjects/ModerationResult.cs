using System.Text.Json.Serialization;
using AdService.Domain.Enums;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record ModerationResult
{
    public const int MAX_MESSAGE_LENGTH = 200;

    public Guid ModeratorId { get; private set; }

    public DateTime DecidedAt { get; private set; }

    public DenyReason? DenyReason { get; private set; }

    public string? Message { get; private set; }

    public bool IsAccepted => DenyReason is null;

    protected ModerationResult()
    {
    }

    [JsonConstructor]
    private ModerationResult(Guid moderatorId, DateTime decidedAt, DenyReason? denyReason = null, string? message = null)
    {
        ModeratorId = moderatorId;
        DecidedAt = decidedAt;
        DenyReason = denyReason;
        Message = message;
    }

    public static Result<ModerationResult, Error> Of(
        Guid moderatorId,
        DateTime decidedAt,
        DenyReason? denyReason = null,
        string? message = null)
    {
        if (message is not null && message.Length > MAX_MESSAGE_LENGTH)
        {
            return Result.Failure<ModerationResult, Error>(Error.Domain(
                "moderation_result.message.is_conflict",
                $"Display name must be less than or equal to {MAX_MESSAGE_LENGTH}."));
        }

        ModerationResult result = new(moderatorId, decidedAt, denyReason, message);
        return Result.Success<ModerationResult, Error>(result);
    }
}