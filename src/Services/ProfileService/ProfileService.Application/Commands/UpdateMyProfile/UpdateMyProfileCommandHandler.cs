using System.Net;
using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Abstractions.IdentityProvider;
using ProfileService.Application.Dtos;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Application.Commands.UpdateMyProfile;

public class UpdateMyProfileCommandHandler(
    IAppDbContext dbContext,
    IAdminKeycloakService keycloakService,
    ILogger<UpdateMyProfileCommandHandler> logger)
    : ICommandHandler<UpdateMyProfileCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(
        UpdateMyProfileCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.UserProfiles
            .SingleOrDefaultAsync(x => x.KeycloakId == command.KeycloakId, cancellationToken);

        if (profile is null)
            return UnitResult.Failure<List<Error>>(Error.NotFound("profile", "User profile not found."));

        var data = command.Profile;
        var emailResult = Email.Of(data.Email);
        if (emailResult.IsFailure)
            return UnitResult.Failure<List<Error>>(emailResult.Error);

        var phoneNumberResult = PhoneNumber.Of(data.PhoneNumber);
        if (phoneNumberResult.IsFailure)
            return UnitResult.Failure<List<Error>>(phoneNumberResult.Error);

        var usernameExists = await dbContext.UserProfiles
            .AnyAsync(x => x.Id != profile.Id && x.Username == data.Username, cancellationToken);

        if (usernameExists)
            return UnitResult.Failure<List<Error>>(Error.Conflict("username", "Username is already in use."));

        var updateResult = profile.ReplaceProfile(
            data.Username, emailResult.Value, data.Name, data.Surname, phoneNumberResult.Value, data.Picture);

        if (updateResult.IsFailure)
            return UnitResult.Failure<List<Error>>(updateResult.Error);


        await keycloakService.UpdateUserAsync(
            command.KeycloakId,
            new UpdateKeycloakProfileDto(data.Name, data.Surname, emailResult.Value.Value, data.Username),
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return UnitResult.Success<List<Error>>();
    }
}