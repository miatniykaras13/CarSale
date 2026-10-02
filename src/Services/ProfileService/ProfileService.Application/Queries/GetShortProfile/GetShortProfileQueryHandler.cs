using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetShortProfile;

public class GetShortProfileQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetShortProfileQuery, Result<ShortProfileDto, List<Error>>>
{
    public async Task<Result<ShortProfileDto, List<Error>>> Handle(
        GetShortProfileQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .AsNoTracking()
            .Where(x => x.Id == query.ProfileId)
            .Select(x => new ShortProfileDto(x.Id, x.Username, x.Picture))
            .SingleOrDefaultAsync(cancellationToken);

        if (profile is null)
            return Result.Failure<ShortProfileDto, List<Error>>([Error.NotFound("profile", "User profile not found.")]);

        return Result.Success<ShortProfileDto, List<Error>>(profile);
    }
}
