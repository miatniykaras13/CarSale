using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record PhoneNumber
{
    public CountryCode CountryCode { get; private set; } = null!;

    public string E164 { get; private set; } = null!;

    protected PhoneNumber()
    {
    }

    private PhoneNumber(CountryCode countryCode, string e164)
    {
        CountryCode = countryCode;
        E164 = e164;
    }

    public static Result<PhoneNumber, Error> Of(CountryCode countryCode, string e164)
    {
        if (e164[0] != '+' || e164.Length is < 9 or > 16)
        {
            return Result.Failure<PhoneNumber, Error>(Error.Domain("phone_number.is.conflict", "Invalid phone number"));
        }

        /*var phoneUtil = PhoneNumberUtil.GetInstance();
        var phoneNumber = phoneUtil.Parse(number, null);

        int countryCode = phoneNumber.CountryCode;*/ // на фабрику потом

        PhoneNumber phone = new(countryCode, e164);
        return Result.Success<PhoneNumber, Error>(phone);
    }
}