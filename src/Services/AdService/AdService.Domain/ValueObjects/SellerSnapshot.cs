using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record SellerSnapshot
{
    public const int MAX_NAME_LENGTH = 50;

    public Guid SellerId { get; private set; }

    public string DisplayName { get; private set; } = null!;

    public DateTime RegistrationDate { get; private set; }

    public Guid ImageId { get; private set; }

    protected SellerSnapshot()
    {
    }

    private SellerSnapshot(Guid sellerId, string displayName, DateTime registrationDate, Guid imageId)
    {
        SellerId = sellerId;
        DisplayName = displayName;
        RegistrationDate = registrationDate;
        ImageId = imageId;
    }


    public static Result<SellerSnapshot, Error> Of(Guid sellerId, string displayName, DateTime registrationDate, Guid imageId)
    {
        if (displayName.Length > MAX_NAME_LENGTH)
        {
            return Result.Failure<SellerSnapshot, Error>(Error.Domain(
                "seller_snapshot.display_name.is.conflict",
                $"Display name must be less than or equal to {MAX_NAME_LENGTH}."));
        }

        SellerSnapshot snapshot = new(sellerId, displayName, registrationDate, imageId);
        return Result.Success<SellerSnapshot, Error>(snapshot);
    }
}