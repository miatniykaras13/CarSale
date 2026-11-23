using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetMyProfile;

public record GetMyProfileQuery(Guid KeycloakId) : IQuery<Result<MyProfileDto, List<Error>>>;
