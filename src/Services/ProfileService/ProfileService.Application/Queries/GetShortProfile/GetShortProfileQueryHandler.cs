using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Abstractions.FileStorage;
using ProfileService.Application.Dtos;
using ProfileService.Application.Options;

namespace ProfileService.Application.Queries.GetShortProfile;

public class GetShortProfileQueryHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage,
    IOptions<FileStorageOptions> fileStorageOptions)
    : IQueryHandler<GetShortProfileQuery, Result<ShortProfileDto, List<Error>>>
{
    public async Task<Result<ShortProfileDto, List<Error>>> Handle(
        GetShortProfileQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .AsNoTracking()
            .Where(x => x.Id == query.ProfileId)
            .Select(x => new { x.Id, x.Username, x.Picture })
            .SingleOrDefaultAsync(cancellationToken);

        if (profile is null)
            return Result.Failure<ShortProfileDto, List<Error>>([Error.NotFound("profile", "User profile not found.")]);

        var picture = profile.Picture.HasValue
            ? await fileStorage.GetDownloadLinkAsync(
                profile.Picture.Value, fileStorageOptions.Value.DownloadLinkExpirySeconds, cancellationToken)
            : null;

        return Result.Success<ShortProfileDto, List<Error>>(new ShortProfileDto(profile.Id, profile.Username, picture));
    }
}