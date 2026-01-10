using AdService.Application.Abstractions.UserProfiles;
using AdService.Contracts.Ads;
using AdService.Contracts.Ads.Default;
using AdService.Contracts.UserProfiles;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace AdService.Infrastructure.ProfileService;

public class ProfileServiceClient : IProfileServiceClient
{
    public async Task<Result<UserProfileForAdDto, Error>> GetUserProfileAsync(Guid userId)
    {
        return Result.Success<UserProfileForAdDto, Error>(new UserProfileForAdDto(
            userId,
            "Vlad",
            DateTime.UtcNow,
            Guid.CreateVersion7(),
            new PhoneNumberDto("+375297304300")));
    }
}