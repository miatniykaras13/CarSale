using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.UserProfiles;
using AdService.Domain.Aggregates;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace AdService.Application.Commands.CreateAd;

public class CreateAdCommandHandler(
    IAppDbContext dbContext,
    IProfileServiceClient profileService) : ICommandHandler<CreateAdCommand, Result<CreateAdResponse, List<Error>>>
{
    public async Task<Result<CreateAdResponse, List<Error>>> Handle(
        CreateAdCommand command,
        CancellationToken cancellationToken)
    {
        var userId = command.UserId;

        var userAds = await dbContext.Ads
            .Where(a => a.Seller.SellerId == userId).ToListAsync(cancellationToken);

        if (userAds.Count >= 3)
        {
            return Result.Failure<CreateAdResponse, List<Error>>(Error.Conflict(
                "user_ads.count",
                "User cannot create more than 3 ads."));
        }

        var userDraft = userAds.FirstOrDefault(a => a.Status == AdStatus.DRAFT);

        if (userDraft is not null)
            return Result.Success<CreateAdResponse, List<Error>>(new CreateAdResponse(userDraft.Id, AdExisted: true));


        var userDtoResult = await profileService.GetUserProfileAsync(userId);

        if (userDtoResult.IsFailure)
            return Result.Failure<CreateAdResponse, List<Error>>(userDtoResult.Error);

        var userDto = userDtoResult.Value;

        var sellerSnapshotResult = SellerSnapshot.Of(
            userId,
            userDto.DisplayName,
            userDto.RegisteredAt,
            userDto.ImageId);

        if (sellerSnapshotResult.IsFailure)
            return Result.Failure<CreateAdResponse, List<Error>>(sellerSnapshotResult.Error);

        var adResult = Ad.Create(sellerSnapshotResult.Value);

        if (adResult.IsFailure)
            return Result.Failure<CreateAdResponse, List<Error>>(adResult.Error);

        var ad = adResult.Value;

        await dbContext.Ads.AddAsync(ad, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success<CreateAdResponse, List<Error>>(new CreateAdResponse(adResult.Value.Id, AdExisted: false));
    }
}