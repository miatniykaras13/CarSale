using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetMyAds;

public record GetMyAdsQuery(Guid KeycloakId) : IQuery<Result<List<AdSnapshotDto>, List<Error>>>;
