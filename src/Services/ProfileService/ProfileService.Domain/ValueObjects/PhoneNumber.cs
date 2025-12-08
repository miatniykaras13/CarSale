using System.Text.RegularExpressions;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Domain.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex _phoneRegex = new Regex(@"^\+[1-9]\d{1-14}", RegexOptions.Compiled);

    public string E164 { get; private set; } = null!;

    protected PhoneNumber()
    {
    }

    private PhoneNumber(string e164)
    {
        E164 = e164;
    }

    public static Result<PhoneNumber, Error> Of(string e164)
    {
        if (!_phoneRegex.IsMatch(e164))
        {
            return Result.Failure<PhoneNumber, Error>(Error.Domain(
                "phone_number.is_conflict",
                "Phone number doesn't match the pattern."));
        }

        PhoneNumber phone = new(e164);
        return Result.Success<PhoneNumber, Error>(phone);
    }
}