using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex _phoneRegex = new(@"^\+[1-9]\d{5,14}", RegexOptions.Compiled);

    public string E164 { get; private set; } = "+000000000000";

    protected PhoneNumber()
    {
    }

    [JsonConstructor]
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