using System.Text.Json.Serialization;
using AdService.Domain.Ads.Enums;
using AdService.Domain.Ads.ValueObjects;
using AdService.Domain.Events;
using AdService.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace AdService.Domain.Ads.Aggregates;

public class Ad : Aggregate<Guid>
{
    public const int MAX_TITLE_LENGTH = 100;
    public const int MIN_TITLE_LENGTH = 10;
    public const int MAX_DESCRIPTION_LENGTH = 400;
    public const int MAX_COMMENT_LENGTH = 100;

    private readonly List<Guid> _images = [];
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
        long views,
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

    public string? Title { get; private set; }

    public string? Description { get; private set; }

    public Money? Price { get; private set; }

    public Location? Location { get; private set; }

    public long Views { get; private set; }

    public Guid SellerId { get; private set; }

    public SellerSnapshot? SellerSnapshot { get; private set; }

    public CarSnapshot? Car { get; private set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarConfiguration? CarConfiguration { get; private set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AdStatus Status { get; private set; }

    public InactiveReason? InactiveReason { get; private set; }

    public ModerationResult? ModerationResult { get; private set; }

    public IReadOnlyList<Guid> Images => _images.AsReadOnly();

    public IReadOnlyList<string> Comments => _comments.AsReadOnly();

    public bool IsActive => InactiveReason is null;


    // First state - draft
    public static Result<Ad, Error> Create(Guid sellerId)
    {
        var ad = new Ad { SellerId = sellerId, Status = AdStatus.DRAFT };

        ad.AddDomainEvent(new AdCreatedEvent(ad));
        return Result.Success<Ad, Error>(ad);
    }


    // Can update any type of ad
    public Result<Ad, Error> Update(
        string? title,
        string? description,
        Money? price,
        Location? location,
        List<Guid>? images,
        CarSnapshot? car,
        SellerSnapshot? seller,
        CarConfiguration? carConfiguration)
    {
        if (title is { Length: > MAX_TITLE_LENGTH or < MIN_TITLE_LENGTH })
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.title.is_conflict",
                $"Title's length must be between {MIN_TITLE_LENGTH} and {MAX_TITLE_LENGTH}"));
        }

        if (description is { Length: > MAX_DESCRIPTION_LENGTH or < 0 })
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.description.is_conflict",
                $"Description's length must be between 0 and {MAX_DESCRIPTION_LENGTH}"));
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


    // Publishing only if moderated and not published yet
    public Result<Unit, Error> Publish()
    {
        if (Status == AdStatus.PUBLISHED)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.status.is_conflict",
                $"Ad's status must not be published to publish."));
        }

        if (ModerationResult is null || !ModerationResult.IsAccepted)
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "moderation_result.is_conflict",
                $"Ad should be moderated with success."));
        }

        Status = AdStatus.PUBLISHED;
        InactiveReason = null;

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

        Status = AdStatus.INACTIVE;
        InactiveReason = Enums.InactiveReason.DENIED;
        ModerationResult = result;

        AddDomainEvent(new AdDeniedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    // cannot add existing images
    public Result<Unit, Error> AddImages(IList<Guid> images)
    {
        if (images.Any(i => _images.Contains(i)))
        {
            return Result.Failure<Unit, Error>(Error.Domain(
                "ad.images.already_exist",
                "Such images already exist"));
        }

        _images.AddRange(images);
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