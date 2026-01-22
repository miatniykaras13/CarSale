using AdService.Contracts.Ads;
using AdService.Contracts.Ads.Default;

namespace AdService.Contracts.UserProfiles;

public record UserProfileForAdDto(Guid UserId, string DisplayName, DateTime RegisteredAt, Guid ImageId, PhoneNumberDto PhoneNumber);