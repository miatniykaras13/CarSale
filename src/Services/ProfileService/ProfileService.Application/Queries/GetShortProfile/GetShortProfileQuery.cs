using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetShortProfile;

public record GetShortProfileQuery(Guid ProfileId) : IQuery<Result<ShortProfileDto, List<Error>>>;
