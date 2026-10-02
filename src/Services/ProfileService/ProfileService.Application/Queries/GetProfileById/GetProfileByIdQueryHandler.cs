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
            .Where(x => x.Id == query.ProfileId)
            .Select(x => new ProfileByIdDto(
                x.Id,
                x.Username,
                x.Email.Value,
                x.Name,
                x.Surname,
                x.PhoneNumber.E164))
            .SingleOrDefaultAsync(cancellationToken);

        if (profile is null)
            return Result.Failure<ProfileByIdDto, List<Error>>([Error.NotFound("profile", "User profile not found.")]);

        return Result.Success<ProfileByIdDto, List<Error>>(profile);
    }
}
