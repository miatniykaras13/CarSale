using System.Text.Json.Serialization;
using AdService.Domain.Ads.Enums;
using AdService.Domain.Ads.ValueObjects;
using AdService.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace AdService.Domain.Ads.Aggregates;

public class Ad : Aggregate<Guid>
{
    private const int MAX_TITLE_LENGTH = 100;
    private const int MIN_TITLE_LENGTH = 10;
    private const int MAX_DESCRIPTION_LENGTH = 400;

    private readonly List<Guid> _images = [];
    private readonly List<Guid> _comments = [];

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

    public SellerSnapshot SellerSnapshot { get; private set; }

    public CarSnapshot? Car { get; private set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CarConfiguration? CarConfiguration { get; private set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AdStatus Status { get; private set; }
    
    public Guid? DenyReasonId { get; private set; } // причина отказа в публикации
    
    public List<Guid> Reports { get; private set; } // жалобы на объявление

    public IReadOnlyList<Guid> Images => _images.AsReadOnly();

    public IReadOnlyList<Guid> Comments => _comments.AsReadOnly();

    public static int MaxTitleLength => MAX_TITLE_LENGTH;

    public static int MinTitleLength => MIN_TITLE_LENGTH;

    public static int MaxDescriptionLength => MAX_DESCRIPTION_LENGTH;

    public static Result<Ad, Error> CreateDraft(Guid sellerId)
    {
        var ad = new Ad { SellerId = sellerId, Status = AdStatus.DRAFT, };

        ad.AddDomainEvent(AdCreatedEvent(ad));
        return Result.Success<Ad, Error>(ad);
    }


    public Result<Ad, Error> UpdateDraft(
        string? title,
        string? description,
        Money? price,
        Location? location,
        List<Guid>? images,
        CarSnapshot? car,
        CarConfiguration? carConfiguration)
    {
        if (Status != AdStatus.DRAFT)
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.status.is_not_draft",
                $"Ad status must be draft to update "));
        }

        if (title is { Length: > MAX_TITLE_LENGTH or < MIN_TITLE_LENGTH })
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.title.is.conflict",
                $"Title's length must be between {MIN_TITLE_LENGTH} and {MAX_TITLE_LENGTH}"));
        }

        if (description is { Length: > MAX_DESCRIPTION_LENGTH or < 0 })
        {
            return Result.Failure<Ad, Error>(Error.Domain(
                "ad.description.is.conflict",
                $"Description's length must be between 0 and {MAX_DESCRIPTION_LENGTH}"));
        }

        Title = title;
        Description = description;
        Price = price;
        Location = location;
        if (images != null)
            AddImages(images);
        Car = car;
        CarConfiguration = carConfiguration;

        AddDomainEvent(AdUpdatedEvent(this));
        return Result.Success<Ad, Error>(this);
    }

    public Result<Unit, Error> Publish()
    {
        if(!Status.Equals(AdStatus.PENDING))
            return Result.Failure<Unit, Error>(Error.Domain("ad.status.is_not_pending", "Ad's status must be pending to publish."));
        Status = AdStatus.PUBLISHED;
        AddDomainEvent(AdPublishedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<Unit, Error> Submit()
    {
        if(!Status.Equals(AdStatus.DRAFT))
            return Result.Failure<Unit, Error>(Error.Domain("ad.status.is_not_draft", "Ad's status must be pending to submit."));
        Status = AdStatus.PENDING;
        AddDomainEvent(AdSubmittedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<Unit, Error> Delete()
    {
        if(!Status.Equals(AdStatus.DRAFT))
            return Result.Failure<Unit, Error>(Error.Domain("ad.status.is_not_draft", "Ad's status must be pending to submit."));
        Status = AdStatus.DELETED;
        AddDomainEvent(AdDeletedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<Unit, Error> Deny()
    {
        if(!Status.Equals(AdStatus.PENDING))
            return Result.Failure<Unit, Error>(Error.Domain("ad.status.is_not_pending", "Ad's status must be pending to deny."));
        Status = AdStatus.DENIED;
        AddDomainEvent(AdDeniedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }


    public Result<Unit, Error> Remove()
    {
        if(!Status.Equals(AdStatus.PENDING))
            return Result.Failure<Unit, Error>(Error.Domain("ad.status.is_not_pending", "Ad's status must be pending to deny."));
        Status = AdStatus.REMOVED;
        AddDomainEvent(AdRemovedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<Unit, Error> Archive()
    {
        Status = AdStatus.ARCHIVED;
        AddDomainEvent(AdArchivedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<Unit, Error> Pause()
    {
        Status = AdStatus.PAUSED;
        AddDomainEvent(AdPausedEvent(this));
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public Result<IEnumerable<Guid>, Error> AddImages(IEnumerable<Guid> images)
    {
        images = images.Distinct();
        if (images.Any(i => _images.Contains(i)))
            return Result.Failure<IEnumerable<Guid>, Error>(Error.Domain("images.already.exist", "Such images already exist"));
        _images.AddRange(images);
        return Result.Success<IEnumerable<Guid>, Error>(images);
    }

    public Result<Guid, Error> AddComment(Guid comment)
    {
        if(_comments.Contains(comment))
        {
            return Result.Failure<Guid, Error>(Error.Domain(
                "comment.already.exists",
                $"Comment with id {comment} already exists"));
        }

        _comments.Add(comment);
        return Result.Success<Guid, Error>(comment);
    }
}