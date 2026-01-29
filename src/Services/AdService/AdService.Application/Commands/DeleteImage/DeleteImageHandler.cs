using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Application.Commands.DeleteImage;
using Microsoft.Extensions.Options;

namespace AdService.Application.Commands.DeleteImage;

public class DeleteImageHandler(
    IFileStorage fileStorage,
    IAppDbContext dbContext) : ICommandHandler<DeleteImageCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(DeleteImageCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

        if (ad is null)
            return UnitResult.Failure<List<Error>>(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        if (ad.Seller.SellerId != command.UserId)
        {
            return UnitResult.Failure<List<Error>>(Error.Forbidden(
                "ad",
                $"Authenticated user does not own the ad"));
        }

        await fileStorage.DeleteFileAsync(command.ImageId, ct);

        var adImageResult = ad.RemoveImage(command.ImageId);

        if (adImageResult.IsFailure) return UnitResult.Failure<List<Error>>(adImageResult.Error);

        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<List<Error>>();
    }
}