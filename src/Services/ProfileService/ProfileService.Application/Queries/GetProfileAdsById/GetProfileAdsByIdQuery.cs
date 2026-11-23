using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetProfileAdsById;

public record GetProfileAdsByIdQuery(Guid ProfileId) : IQuery<Result<List<AdSnapshotDto>, List<Error>>>;
