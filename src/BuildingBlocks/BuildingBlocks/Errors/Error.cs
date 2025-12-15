using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Routing.Template;

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

    /// <summary>
    /// Represents validation error
    /// </summary>
    /// <param name="obj">invalid object.</param>
    /// <param name="message">additional info.</param>
    /// <returns>Error.</returns>
    public static Error Validation(string obj, string? message = null) =>
        new Error($"{obj}.is_invalid", ErrorType.VALIDATION, message);


    /// <summary>
    /// Represents not found error
    /// </summary>
    /// <param name="obj">not found obj.</param>
    /// <param name="message">additional info.</param>
    /// <returns>Error.</returns>
    public static Error NotFound(string obj, string? message = null) =>
        new Error($"{obj}.not_found", ErrorType.NOT_FOUND, message);

    /// <summary>
    /// Represents internal server error
    /// </summary>
    /// <param name="message">additional info.</param>
    /// <returns>Error.</returns>
    public static Error Internal(string? message = null) =>
        new Error($"internal_error", ErrorType.INTERNAL, message);

    /// <summary>
    /// Represents conflict error
    /// </summary>
    /// <param name="obj">conflicting obj.</param>
    /// <param name="message">additional info.</param>
    /// <returns>Error.</returns>
    public static Error Conflict(string obj, string? message = null) =>
        new Error($"{obj}.is_conflict", ErrorType.CONFLICT, message);

    /// <summary>
    /// Represents unknown error
    /// </summary>
    /// <param name="message">additional info.</param>
    /// <returns>Error.</returns>
    public static Error Unknown(string? message = null) =>
        new Error($"unknown_error", ErrorType.UNKNOWN, message);

    /// <summary>
    /// Represents domain error. Error code should be fully entered
    /// </summary>
    /// <param name="code">error code.</param>
    /// <param name="message">additional info.</param>
    /// <returns>Error.</returns>
    public static Error Domain(string code, string? message = null) =>
        new Error(code, ErrorType.CONFLICT, message);

    public static implicit operator List<Error>(Error error) => [error];
}