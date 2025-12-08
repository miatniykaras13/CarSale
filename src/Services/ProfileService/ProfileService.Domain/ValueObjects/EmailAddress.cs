using System.Text.RegularExpressions;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Domain.ValueObjects;

public record EmailAddress
{
    private static readonly Regex _emailRegex =
        new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; private set; } = null!;

    protected EmailAddress()
    {
    }

    private EmailAddress(string value)
    {
        Value = value;
    }

    public static Result<EmailAddress, Error> Of(string email)
    {
        if (!_emailRegex.Matches(email).Any())
        {
            return Result.Failure<EmailAddress, Error>(Error.Domain(
                "email.is_conflict",
                "Email address doesn't follow the pattern."));
        }

        return Result.Success<EmailAddress, Error>(new EmailAddress(email));
    }
}