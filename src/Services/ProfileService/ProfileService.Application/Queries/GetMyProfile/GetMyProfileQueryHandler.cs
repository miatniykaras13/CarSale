using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetMyProfile;

public class GetMyProfileQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetMyProfileQuery, Result<MyProfileDto, List<Error>>>
{
    public async Task<Result<MyProfileDto, List<Error>>> Handle(
        GetMyProfileQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .AsNoTracking()
            .Where(x => x.KeycloakId == query.KeycloakId)
            .Select(x => new MyProfileDto(
                x.Id,
                x.Username,
                x.Email.Value,
                x.Name,
                x.Surname,
                x.PhoneNumber.E164))
            .SingleOrDefaultAsync(cancellationToken);

        if (profile is null)
            return Result.Failure<MyProfileDto, List<Error>>([Error.NotFound("profile", "User profile not found.")]);

        return Result.Success<MyProfileDto, List<Error>>(profile);
    }
}
