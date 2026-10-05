using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Abstractions.IdentityProvider;

namespace ProfileService.Application.Commands.DeleteMyProfile;

public class DeleteMyProfileCommandHandler(
    IAppDbContext dbContext,
    IAdminKeycloakService keycloakService)
    : ICommandHandler<DeleteMyProfileCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(
        DeleteMyProfileCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .SingleOrDefaultAsync(x => x.KeycloakId == command.KeycloakId, cancellationToken);

        if (profile is null)
            return UnitResult.Failure<List<Error>>(Error.NotFound("profile", "User profile not found."));

        dbContext.UserProfiles.Remove(profile);

        await keycloakService.DeleteUserAsync(command.KeycloakId, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<List<Error>>();
    }
}