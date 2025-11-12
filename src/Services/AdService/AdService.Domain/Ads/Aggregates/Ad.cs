using System.Text.Json.Serialization;
using AdService.Domain.Ads.Enums;
using AdService.Domain.Ads.ValueObjects;
using AdService.Domain.Events;
using AdService.Domain.Shared.ValueObjects;
using BuildingBlocks.DDD.Abstractions;
using CSharpFunctionalExtensions;
using MediatR;

namespace AdService.Domain.Ads.Aggregates;

public class Ad : Aggregate<Guid>
{
    public const int MAX_TITLE_LENGTH = 100;
    public const int MIN_TITLE_LENGTH = 10;
    public const int MAX_DESCRIPTION_LENGTH = 400;
    public const int MAX_COMMENT_LENGTH = 100;

    private readonly HashSet<Guid> _images = [];
    private readonly List<string> _comments = [];

    private Ad()
        : base(Guid.CreateVersion7())
    {
    }

    private Ad(
        string title,
        string description,
        Money price,
        Location location,
        int views,
        Guid sellerId,
        CarSnapshot car,
        CarConfiguration carConfiguration,
        SellerSnapshot seller,
        AdStatus status)
        : this()
    {
        Title = title;
        Description = description;
        Price = price;
        Location = location;
        Views = views;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        SellerId = sellerId;
        SellerSnapshot = seller;
        Car = car;
        CarConfiguration = carConfiguration;
        Status = status;
    }

    public string Title { get; private set; }

    public string? Description { get; private set; }

    public Money? Price { get; private set; }

    public Location? Location { get; private set; }

    public int Views { get; private set; }

    public Guid SellerId { get; private set; }

    public SellerSnapshot? SellerSnapshot { get; private set; }

    public CarSnapshot? Car { get; private set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarConfiguration? CarConfiguration { get; private set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AdStatus Status { get; private set; }

    public ModerationResult? ModerationResult { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt <= DateTime.UtcNow;

    public IReadOnlyCollection<Guid> Images => _images;

    public IReadOnlyList<string> Comments => _comments.AsReadOnly();

    // First state - draft
    public static Result<Ad, Error> Create(Guid sellerId)
    {
        var ad = new Ad
        {
            SellerId = sellerId, Status = AdStatus.DRAFT, CreatedAt = DateTime.UtcNow, CreatedBy = sellerId.ToString(),
        };

        ad.AddDomainEvent(new AdCreatedEvent(ad));
        return Result.Success<Ad, Error>(ad);
    }

    // Can update only draft, denied, paused and published
    public Result<Ad, Error> Update(
        string? title,
        string? description,
        Money? price,
        Location? location,
        IEnumerable<Guid>? images,
        CarSnapshot? car,
        SellerSnapshot? seller,
        CarConfiguration? carConfiguration)
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

        Title = title;
        Description = description;
        Price = price;
        Location = location;
        if (images != null)
        {
            var imagesResult = AddImages(images);
            if (imagesResult.IsFailure)
                return imagesResult.Error;
        }

        Car = car;
        SellerSnapshot = seller;
        CarConfiguration = carConfiguration;

        AddDomainEvent(new AdUpdatedEvent(this));
        return Result.Success<Ad, Error>(this);
    }

    // can clear draft or denied
    public Result<Unit, Error> Clear()
    {
        if (Status is not (AdStatus.DRAFT or AdStatus.DENIED))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be draft or denied to be cleared"));
        }

        Title = null;
        Description = null;
        Price = null;
        Location = null;
        Car = null;
        SellerSnapshot = null;
        CarConfiguration = null;

        _images.Clear();
        _comments.Clear();

        AddDomainEvent(new AdClearedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // can submit only with status denied or draft
    public Result<Unit, Error> Submit()
    {
        if (Status is not (AdStatus.DRAFT or AdStatus.DENIED))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be draft or denied to submit."));
        }

        if (Title is { Length: > MAX_TITLE_LENGTH or < MIN_TITLE_LENGTH })
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.title.is_conflict",
                $"Title's length must be between {MIN_TITLE_LENGTH} and {MAX_TITLE_LENGTH} to submit"));
        }

        if (_images.Count == 0)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.images.is_conflict",
                "Ad cannot be submitted without any images"));
        }

        if (Car is null)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.car_snapshot.is_conflict",
                "Ad cannot be submitted without the car information"));
        }

        if (Price is null)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.price.is_conflict",
                "Ad cannot be submitted without the price"));
        }

        if (SellerSnapshot is null)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.seller_snapshot.is_conflict",
                "Ad cannot be submitted without the information about the seller"));
        }

        if (Location is null)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.location.is_conflict",
                "Ad cannot be submitted without the location"));
        }

        Status = AdStatus.PENDING;

        AddDomainEvent(new AdSubmittedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // can cancel submitting only with status pending
    public Result<Unit, Error> CancelSubmission()
    {
        if (Status != AdStatus.PENDING)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be pending to cancel submitting."));
        }

        Status = AdStatus.DRAFT;

        AddDomainEvent(new AdSubmissionCanceledEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // Publishing only if moderated and not published yet
    public Result<Unit, Error> Publish(TimeSpan lifespan)
    {
        if (Status is not (AdStatus.PENDING or AdStatus.PAUSED))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be pending or paused to publish."));
        }

        if (ModerationResult is null || !ModerationResult.IsAccepted)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "moderation_result.is_conflict",
                "Ad is not moderated or denied"));
        }

        Status = AdStatus.PUBLISHED;
        ExpiresAt = DateTime.UtcNow.Add(lifespan);

        AddDomainEvent(new AdPublishedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // Moderator can only deny pending ad
    public Result<Unit, Error> Deny(ModerationResult result)
    {
        if (Status != AdStatus.PENDING)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be pending to deny."));
        }

        if (result.IsAccepted)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "moderation_result.is_conflict",
                $"Moderation result must not be accepted to deny publication"));
        }

        Status = AdStatus.DENIED;
        ModerationResult = result;

        AddDomainEvent(new AdDeniedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // when deleted ad cannot be seen by users and can't be modified
    public Result<Unit, Error> Delete()
    {
        if (IsExpired)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be modified."));
        }

        if (Status is not (AdStatus.PUBLISHED or AdStatus.ARCHIVED or AdStatus.PAUSED))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be published, archived or paused to delete."));
        }

        Status = AdStatus.DELETED;

        AddDomainEvent(new AdDeletedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // when archived it can't be updated, submitted, paused or published
    public Result<Unit, Error> Archive()
    {
        if (IsExpired)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be deleted."));
        }

        if (Status is not (AdStatus.PUBLISHED or AdStatus.PAUSED))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be published or paused to archive."));
        }

        Status = AdStatus.ARCHIVED;

        AddDomainEvent(new AdArchivedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // when paused it is not visible on marketplace
    public Result<Unit, Error> Pause()
    {
        if (IsExpired)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be modified."));
        }

        if (Status != AdStatus.PUBLISHED)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be published to pause."));
        }

        Status = AdStatus.PAUSED;

        AddDomainEvent(new AdPausedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // when sold it is not visible on marketplace
    public Result<Unit, Error> Sell()
    {
        if (IsExpired)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.is_expired",
                "Ad is expired and it cannot be modified."));
        }

        if (Status is not (AdStatus.PUBLISHED or AdStatus.PAUSED))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must be published or paused to sell."));
        }

        Status = AdStatus.PAUSED;

        AddDomainEvent(new AdSoldEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<Unit, Error> Expire()
    {
        if (!IsExpired)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.is_not_expired",
                "Ad is not expired yet."));
        }

        Status = AdStatus.EXPIRED;

        AddDomainEvent(new AdExpiredEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // cannot add existing images
    public Result<Unit, Error> AddImages(IEnumerable<Guid> images)
    {
        if (images.Any(i => _images.Contains(i)))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.images.already_exist",
                "Such images already exist"));
        }

        _images.UnionWith(images);
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<Unit, Error> RemoveImage(Guid image)
    {
        if (!_images.Contains(image))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.image.not_found",
                "Image not found"));
        }

        _images.Remove(image);
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<Unit, Error> AddComment(string comment)
    {
        if (comment.Length > MAX_COMMENT_LENGTH)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.comment.is_conflict",
                $"Comment's length must be less or equal than {MAX_COMMENT_LENGTH}"));
        }

        _comments.Add(comment);
        return Result.Success<Unit, Error>(Unit.Value);
    }
}