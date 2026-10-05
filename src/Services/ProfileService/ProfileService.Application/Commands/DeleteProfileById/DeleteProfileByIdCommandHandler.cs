using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Abstractions.IdentityProvider;

namespace ProfileService.Application.Commands.DeleteProfileById;

public class DeleteProfileByIdCommandHandler(
    IAppDbContext dbContext,
    IAdminKeycloakService keycloakService)
    : ICommandHandler<DeleteProfileByIdCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(
        DeleteProfileByIdCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .SingleOrDefaultAsync(x => x.Id == command.ProfileId, cancellationToken);

        if (profile is null)
            return UnitResult.Failure<List<Error>>(Error.NotFound("profile", "User profile not found."));

        await keycloakService.DeleteUserAsync(profile.KeycloakId, cancellationToken);

        dbContext.UserProfiles.Remove(profile);
        await dbContext.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<List<Error>>();
    }
}
