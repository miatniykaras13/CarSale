using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Abstractions.FileStorage;
using ProfileService.Application.Dtos;
using ProfileService.Application.Options;

namespace ProfileService.Application.Queries.GetMyProfile;

public class GetMyProfileQueryHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage,
    IOptions<FileStorageOptions> fileStorageOptions)
    : IQueryHandler<GetMyProfileQuery, Result<MyProfileDto, List<Error>>>
{
    public async Task<Result<MyProfileDto, List<Error>>> Handle(
        GetMyProfileQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.KeycloakId == query.KeycloakId, cancellationToken);

        if (profile is null)
            return Result.Failure<MyProfileDto, List<Error>>([Error.NotFound("profile", "User profile not found.")]);

        var picture = profile.Picture.HasValue
            ? await fileStorage.GetDownloadLinkAsync(
                profile.Picture.Value, fileStorageOptions.Value.DownloadLinkExpirySeconds, cancellationToken)
            : null;

        var profileDto = new MyProfileDto(
            profile.Id,
            profile.Username,
            profile.Email.Value,
            profile.Name,
            profile.Surname,
            profile.PhoneNumber.E164,
            picture);

        return Result.Success<MyProfileDto, List<Error>>(profileDto);
    }
}