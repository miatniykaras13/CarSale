using AdService.Contracts.UserProfiles;

namespace AdService.Application.Abstractions.UserProfiles;

public interface IProfileServiceClient
{
    Task<Result<UserProfileForAdDto, Error>> GetUserProfileAsync(Guid userId);
}