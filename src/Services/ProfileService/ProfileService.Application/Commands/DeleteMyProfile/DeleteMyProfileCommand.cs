using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Application.Commands.DeleteMyProfile;

public record DeleteMyProfileCommand(Guid KeycloakId) : ICommand<UnitResult<List<Error>>>;
