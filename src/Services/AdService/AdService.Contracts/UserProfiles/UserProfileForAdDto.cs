namespace AdService.Contracts.UserProfiles;

public record UserProfileForAdDto(Guid UserId, string DisplayName, DateTime RegisteredAt, Guid ImageId);