using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Domain.ValueObjects;

public record Email
{
    private static readonly Regex _emailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled);

    public string Value { get; private init; } = null!;

    private Email()
    {
    }

    [JsonConstructor]
    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Of(string value)
    {
        if (!_emailRegex.IsMatch(value))
        {
            return Result.Failure<Email, Error>(Error.Domain("email.is_conflict", "Email doesn't match the pattern."));
        }

        var email = new Email(value.ToLowerInvariant());
        return Result.Success<Email, Error>(email);
    }

    public override string ToString() => Value;
}