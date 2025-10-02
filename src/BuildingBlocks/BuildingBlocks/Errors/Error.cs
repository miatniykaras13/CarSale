using System.Text.Json.Serialization;

namespace BuildingBlocks.Errors;

public class Error
{
    public string Code { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ErrorType Type { get; init; }

    public string? Message { get; init; }

    [JsonConstructor]
    private Error(string code, ErrorType type, string? message = null)
    {
        Code = code;
        Type = type;
        Message = message;
    }

    public static Error Validation(string obj, string? message = null) =>
        new Error($"{obj}.is.invalid", ErrorType.VALIDATION, message);

    public static Error NotFound(string obj, string? message = null) =>
        new Error($"{obj}.not.found", ErrorType.NOT_FOUND, message);

    public static Error Internal(string obj, string? message = null) =>
        new Error($"internal.error", ErrorType.INTERNAL, message);

    public static Error Conflict(string obj, string? message = null) =>
        new Error($"{obj}.is.conflict", ErrorType.CONFLICT, message);

    public static Error Unknown(string? message = null) =>
        new Error($"unknown.error", ErrorType.UNKNOWN, message);
}