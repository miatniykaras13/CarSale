using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Commands.UpdateMyProfile;

public record UpdateMyProfileCommand(Guid KeycloakId, UpdateMyProfileDto Profile) : ICommand<UnitResult<List<Error>>>;
