using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Application.Commands.UpdateMyProfilePicture;

public record UpdateMyProfilePictureCommand(
    Guid KeycloakId,
    Stream Stream,
    string FileName,
    string ContentType) : ICommand<Result<Guid, List<Error>>>;