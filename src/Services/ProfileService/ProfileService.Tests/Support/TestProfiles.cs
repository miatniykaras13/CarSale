using ProfileService.Domain.Aggregates;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Tests.Support;

internal static class TestProfiles
{
    public static UserProfile Create(string username = "seller", Guid? pictureId = null)
    {
        var profile = UserProfile.Create(
            Guid.NewGuid(),
            Email.Of($"{username}@example.com").Value,
            username,
            "Ivan",
            "Ivanov",
            PhoneNumber.Of("+375291234567").Value,
            pictureId).Value;

        profile.UpdatedAt = profile.CreatedAt;
        profile.UpdatedBy = username;
        return profile;
    }

    public static AdSnapshot CreateAd(string adId, bool withDetails = true)
    {
        var car = withDetails
            ? CarSnapshot.Of("BMW", "3 Series", "F30", 2015, "RWD", "Automatic", 2.0, "Petrol", "Sedan").Value
            : null;
        var price = withDetails ? Money.Of(Currency.Of("USD").Value, 15000).Value : null;

        return AdSnapshot.Of(
            adId, withDetails ? "PUBLISHED" : "DRAFT", withDetails ? "Car for sale" : null, car, price).Value;
    }
}
