using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Abstractions.FileStorage;
using ProfileService.Application.Dtos;
using ProfileService.Application.Options;

namespace ProfileService.Application.Queries.GetProfileById;

public class GetProfileByIdQueryHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage,
    IOptions<FileStorageOptions> fileStorageOptions)
    : IQueryHandler<GetProfileByIdQuery, Result<ProfileByIdDto, List<Error>>>
{
    public async Task<Result<ProfileByIdDto, List<Error>>> Handle(
        GetProfileByIdQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.ProfileId, cancellationToken);

        if (profile is null)
            return Result.Failure<ProfileByIdDto, List<Error>>([Error.NotFound("profile", "User profile not found.")]);

        var picture = profile.Picture.HasValue
            ? await fileStorage.GetDownloadLinkAsync(
                profile.Picture.Value, fileStorageOptions.Value.DownloadLinkExpirySeconds, cancellationToken)
            : null;

        var profileDto = new ProfileByIdDto(
            profile.Id,
            profile.Username,
            profile.Email.Value,
            profile.Name,
            profile.Surname,
            profile.PhoneNumber.E164,
            picture);

        return Result.Success<ProfileByIdDto, List<Error>>(profileDto);
    }
}