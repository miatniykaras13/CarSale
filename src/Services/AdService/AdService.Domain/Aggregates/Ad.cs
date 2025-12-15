using System.Text.Json.Serialization;
using AdService.Domain.Entities;
using AdService.Domain.Enums;
using AdService.Domain.Events;
using AdService.Domain.ValueObjects;
using BuildingBlocks.DDD;
using CSharpFunctionalExtensions;

namespace AdService.Domain.Aggregates;

public sealed class Ad : Aggregate<Guid>
{
    public const int MAX_TITLE_LENGTH = 100;
    public const int MIN_TITLE_LENGTH = 10;
    public const int MAX_DESCRIPTION_LENGTH = 400;

    private readonly List<Guid> _images = [];
    private readonly List<Comment> _comments = [];
    private readonly List<CarOption> _carOptions = [];

    private Ad()
    {
    }

    private Ad(
        string? title,
        string? description,
        Money? price,
        Location? location,
        int views,
        CarSnapshot? car,
        SellerSnapshot? seller,
        AdStatus status)
    {
        Title = title;
        Description = description;
        Price = price;
        Location = location;
        Views = views;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Seller = seller;
        Car = car;
        Status = status;
    }

    public string? Title { get; private set; }

    public string? Description { get; private set; } = null!;

    public Money? Price { get; private set; } = null!;

    public Location? Location { get; private set; } = null!;

    public int Views { get; private set; }

    public SellerSnapshot Seller { get; private set; } = null!;

    public CarSnapshot? Car { get; private set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AdStatus Status { get; private set; }

    public ModerationResult? ModerationResult { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt <= DateTime.UtcNow;

    public IReadOnlyList<Guid> Images => _images.AsReadOnly();

    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

    public IReadOnlyList<CarOption> CarOptions => _carOptions.AsReadOnly();

    // First state - draft
    public static Result<Ad, Error> Create(SellerSnapshot seller)
    {
        var ad = new Ad
        {
            Id = Guid.CreateVersion7(), Seller = seller, Status = AdStatus.DRAFT, CreatedAt = DateTime.UtcNow, CreatedBy = seller.DisplayName,
        };

        ad.AddDomainEvent(new AdCreatedEvent(ad));
        return Result.Success<Ad, Error>(ad);
    }

    // Can update only draft, denied, paused and published
    public Result<Ad, Error> Update(
        string? title = null,
        string? description = null,
        Money? price = null,
        Location? location = null,
        CarSnapshot? car = null,
        SellerSnapshot? seller = null)
    {
        if (IsExpired)
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be modified."));
        }

        if (Status is not (AdStatus.DRAFT or AdStatus.DENIED or AdStatus.PUBLISHED or AdStatus.PAUSED))
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.status.is_conflict",
                "Ad's status must be draft or denied to update"));
        }

        if (title is { Length: > MAX_TITLE_LENGTH or < MIN_TITLE_LENGTH })
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.title.is_conflict",
                $"Title's length must be between {MIN_TITLE_LENGTH} and {MAX_TITLE_LENGTH} to update"));
        }

        if (description is { Length: > MAX_DESCRIPTION_LENGTH })
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.description.is_conflict",
                $"Description's length must be between 0 and {MAX_DESCRIPTION_LENGTH} to update"));
        }

        if (title is not null) Title = title;
        if (description is not null) Description = description;
        if (price is not null) Price = price;
        if (location is not null) Location = location;
        if (car is not null) Car = car;
        if (seller is not null) Seller = seller;


        AddDomainEvent(new AdUpdatedEvent(this));
        return Result.Success<Ad, Error>(this);
    }

    // can clear draft or denied
    public UnitResult<Error> Clear()
    {
        if (Status is not (AdStatus.DRAFT or AdStatus.DENIED))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be draft or denied to be cleared"));
        }

        Title = null;
        Description = null;
        Price = null;
        Location = null;
        Car = null;

        _images.Clear();
        _comments.Clear();
        _carOptions.Clear();

        AddDomainEvent(new AdClearedEvent(this));
        return UnitResult.Success<Error>();
    }

    // can submit only with status denied or draft
    public UnitResult<Error> Submit()
    {
        if (Status is not (AdStatus.DRAFT or AdStatus.DENIED))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be draft or denied to submit."));
        }

        if (Title is { Length: > MAX_TITLE_LENGTH or < MIN_TITLE_LENGTH })
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.title.is_conflict",
                $"Title's length must be between {MIN_TITLE_LENGTH} and {MAX_TITLE_LENGTH} to submit"));
        }

        if (_images.Count == 0)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.images.is_conflict",
                "Ad cannot be submitted without any images"));
        }

        if (Car is null)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.car_snapshot.is_conflict",
                "Ad cannot be submitted without the car information"));
        }

        if (Price is null)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.price.is_conflict",
                "Ad cannot be submitted without the price"));
        }

        if (Location is null)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.location.is_conflict",
                "Ad cannot be submitted without the location"));
        }

        Status = AdStatus.PENDING;

        AddDomainEvent(new AdSubmittedEvent(this));
        return UnitResult.Success<Error>();
    }

    // can cancel submission only with status pending
    public UnitResult<Error> CancelSubmission()
    {
        if (Status != AdStatus.PENDING)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be pending to cancel submitting."));
        }

        Status = AdStatus.DRAFT;

        AddDomainEvent(new AdSubmissionCanceledEvent(this));
        return UnitResult.Success<Error>();
    }

    // Publishing only if moderated and not published yet
    public UnitResult<Error> Publish(TimeSpan lifespan, ModerationResult moderationResult)
    {
        if (Status is not (AdStatus.PENDING or AdStatus.PAUSED))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be pending or paused to publish."));
        }

        if (!moderationResult.IsAccepted)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "moderation_result.is_conflict",
                "Ad is not accepted to be published"));
        }

        Status = AdStatus.PUBLISHED;
        ExpiresAt = DateTime.UtcNow.Add(lifespan);
        ModerationResult = moderationResult;

        AddDomainEvent(new AdPublishedEvent(this));
        return UnitResult.Success<Error>();
    }

    // Moderator can only deny a pending ad
    public UnitResult<Error> Deny(ModerationResult result)
    {
        if (Status != AdStatus.PENDING)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be pending to deny."));
        }

        if (result.IsAccepted)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "moderation_result.is_conflict",
                $"Moderation result must not be accepted to deny publication"));
        }

        Status = AdStatus.DENIED;
        ModerationResult = result;

        AddDomainEvent(new AdDeniedEvent(this));
        return UnitResult.Success<Error>();
    }

    // when deleted ad cannot be seen by users and can't be modified
    public UnitResult<Error> Delete()
    {
        if (IsExpired)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be modified."));
        }

        if (Status is not (AdStatus.PUBLISHED or AdStatus.ARCHIVED or AdStatus.PAUSED))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be published, archived or paused to delete."));
        }

        Status = AdStatus.DELETED;

        AddDomainEvent(new AdDeletedEvent(this));
        return UnitResult.Success<Error>();
    }

    // when archived it can't be updated, submitted, paused or published
    public UnitResult<Error> Archive()
    {
        if (IsExpired)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be deleted."));
        }

        if (Status is not (AdStatus.PUBLISHED or AdStatus.PAUSED))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be published or paused to archive."));
        }

        Status = AdStatus.ARCHIVED;

        AddDomainEvent(new AdArchivedEvent(this));
        return UnitResult.Success<Error>();
    }

    // when paused, it is not visible on a marketplace
    public UnitResult<Error> Pause()
    {
        if (IsExpired)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be modified."));
        }

        if (Status != AdStatus.PUBLISHED)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be published to pause."));
        }

        Status = AdStatus.PAUSED;

        AddDomainEvent(new AdPausedEvent(this));
        return UnitResult.Success<Error>();
    }

    // when sold, it is not visible on a marketplace
    public UnitResult<Error> Sell()
    {
        if (IsExpired)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be modified."));
        }

        if (Status is not (AdStatus.PUBLISHED or AdStatus.PAUSED))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be published or paused to sell."));
        }

        Status = AdStatus.SOLD;

        AddDomainEvent(new AdSoldEvent(this));
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Expire()
    {
        if (Status == AdStatus.EXPIRED)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.already_expired",
                "Ad is already expired."));
        }

        if (!IsExpired)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.is_not_expired",
                "Ad is not expired yet."));
        }

        Status = AdStatus.EXPIRED;

        AddDomainEvent(new AdExpiredEvent(this));
        return UnitResult.Success<Error>();
    }

    // cannot add existing images
    public UnitResult<Error> AddImages(IList<Guid> images)
    {
        if (images.Any(i => _images.Contains(i)))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.images.already_exist",
                "Such images already exist"));
        }

        _images.AddRange(images);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveImage(Guid image)
    {
        if (!_images.Contains(image))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.image.not_found",
                "Image not found"));
        }

        _images.Remove(image);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddComment(Comment comment)
    {
        if (_comments.Contains(comment))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.comment.already_exist",
                "Such comment already exists"));
        }

        _comments.Add(comment);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddCarOptions(IList<CarOption> carOptions)
    {
        if (carOptions.Any(o => _carOptions.Contains(o)))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.car_options.already_exist",
                "Such car options already exist"));
        }

        _carOptions.AddRange(carOptions);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveCarOption(CarOption carOption)
    {
        if (!_carOptions.Contains(carOption))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "ad.car_option.does_not_exist",
                "Such car option does not exist"));
        }

        _carOptions.Remove(carOption);
        return UnitResult.Success<Error>();
    }
}