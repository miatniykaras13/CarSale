using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Application.Dtos;

namespace ProfileService.Application.Queries.GetMyAds;

public class GetMyAdsQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetMyAdsQuery, Result<List<AdSnapshotDto>, List<Error>>>
{
    public async Task<Result<List<AdSnapshotDto>, List<Error>>> Handle(
        GetMyAdsQuery query,
        CancellationToken cancellationToken)
    {
        var ads = await dbContext.UserProfiles
            .AsNoTracking()
            .Where(x => x.KeycloakId == query.KeycloakId)
            .Select(x => x.Ads.Select(ad => new AdSnapshotDto(
                ad.AdId,
                ad.Status,
                ad.Title,
                ad.Car == null ? null : new CarSnapshotDto(
                    ad.Car.Brand,
                    ad.Car.Model,
                    ad.Car.Generation,
                    ad.Car.Year,
                    ad.Car.DriveType,
                    ad.Car.TransmissionType,
                    ad.Car.EngineVolume,
                    ad.Car.FuelType,
                    ad.Car.BodyType),
                ad.Price == null ? null : new MoneyDto(
                    ad.Price.Amount,
                    new CurrencyDto(ad.Price.Currency.Code))))
                .ToList())
            .SingleOrDefaultAsync(cancellationToken);

        if (ads is null)
            return Result.Failure<List<AdSnapshotDto>, List<Error>>([Error.NotFound("profile", "User profile not found.")]);

        return Result.Success<List<AdSnapshotDto>, List<Error>>(ads);
    }
}
