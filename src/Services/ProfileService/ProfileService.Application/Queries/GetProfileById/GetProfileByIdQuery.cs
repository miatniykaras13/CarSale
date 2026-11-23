using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetProfileById;

public record GetProfileByIdQuery(Guid ProfileId) : IQuery<Result<ProfileByIdDto, List<Error>>>;
