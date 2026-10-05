using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetProfileById;

public class GetProfileByIdQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetProfileByIdQuery, Result<ProfileByIdDto, List<Error>>>
{
    public async Task<Result<ProfileByIdDto, List<Error>>> Handle(
        GetProfileByIdQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .AsNoTracking()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.ProfileId, cancellationToken);

        if (profile is null)
            return Result.Failure<ProfileByIdDto, List<Error>>([Error.NotFound("profile", "User profile not found.")]);

        var profileDto = new ProfileByIdDto(
            profile.Id,
            profile.Username,
            profile.Email.Value,
            profile.Name,
            profile.Surname,
            profile.PhoneNumber.E164);

        return Result.Success<ProfileByIdDto, List<Error>>(profileDto);
    }
}