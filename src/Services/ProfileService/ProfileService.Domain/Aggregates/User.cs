using BuildingBlocks.DDD;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Domain.Enums;
using ProfileService.Domain.Events;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Domain.Aggregates;

public sealed class User : Aggregate<Guid>
{
    public const int MIN_DISPLAY_NAME = 5;
    public const int MAX_DISPLAY_NAME = 50;

    private readonly List<Guid> _ads = [];

    private readonly List<string> _searchHistory = [];

    private readonly List<Guid> _bookmarks = []; // избранные объявления

    private readonly List<CarSnapshot> _garage = [];

    public string DisplayName { get; private set; }

    public EmailAddress? Email { get; private set; }

    public Guid? ProfilePhoto { get; private set; }

    public PhoneNumber? PhoneNumber { get; private set; }

    public Address? Address { get; private set; }

    public UserStatus Status { get; private set; }

    public IReadOnlyList<CarSnapshot> Garage => _garage;

    public IReadOnlyList<string> SearchHistory => _searchHistory;

    public IReadOnlyList<Guid> Bookmarks => _bookmarks;

    public IReadOnlyList<Guid> Ads => _ads;


    private User()
    {
    }

    public static Result<User, Error> CreateWithDisplayName(
        Guid userId,
        string displayName)
    {
        if (displayName.Length is < MIN_DISPLAY_NAME or > MAX_DISPLAY_NAME)
        {
            return Result.Failure<User, Error>(Error.Domain(
                "user.display_name.is_conflict",
                $"User display name must be between {MIN_DISPLAY_NAME} and  {MAX_DISPLAY_NAME} characters long."));
        }

        var user = new User { Id = userId, DisplayName = displayName, Status = UserStatus.EMAIL_NOT_VERIFIED, };

        user.AddDomainEvent(new UserCreatedEvent(user));
        return Result.Success<User, Error>(user);
    }

    public static Result<User, Error> CreateWithEmail(
        Guid userId,
        EmailAddress email)
    {
        var user = new User
        {
            Id = userId, DisplayName = email.Value, Email = email, Status = UserStatus.EMAIL_NOT_VERIFIED,
        };

        user.AddDomainEvent(new UserCreatedEvent(user));
        return Result.Success<User, Error>(user);
    }

    public UnitResult<Error> ConfirmEmailVerified()
    {
        if (Status is not UserStatus.EMAIL_NOT_VERIFIED)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "user.status.is_conflict",
                "To verify email address user status must be EMAIL_NOT_VERIFIED."));
        }

        Status = UserStatus.ACTIVE;
        return UnitResult.Success<Error>();
    }


    public UnitResult<Error> Update(
        string? displayName = null,
        EmailAddress? emailAddress = null,
        PhoneNumber? phoneNumber = null,
        Address? address = null)
    {
        if (Status is not (UserStatus.EMAIL_NOT_VERIFIED or UserStatus.ACTIVE))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "user.status.is_conflict",
                "To update user information status must be EMAIL_NOT_VERIFIED or ACTIVE."));
        }

        if (displayName?.Length is < MIN_DISPLAY_NAME or > MAX_DISPLAY_NAME)
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "user.display_name.is_conflict",
                $"User display name must be between {MIN_DISPLAY_NAME} and  {MAX_DISPLAY_NAME} characters long."));
        }

        if (emailAddress is not null)
        {
            Email = emailAddress;
            Status = UserStatus.EMAIL_NOT_VERIFIED;
        }

        if (phoneNumber is not null) PhoneNumber = phoneNumber;
        if (address is not null) Address = address;

        AddDomainEvent(new UserUpdatedEvent(this));
        return UnitResult.Success<Error>();
    }


    public UnitResult<Error> AddCarToGarage(CarSnapshot carSnapshot)
    {
        if (_garage.Contains(carSnapshot))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "garage.car_snapshot.already_exists",
                $"Car snapshot already exists in the garage."));
        }

        _garage.Add(carSnapshot);
        AddDomainEvent(new CarAddedToGarageEvent(carSnapshot.CarId));
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveCarFromGarage(CarSnapshot carSnapshot)
    {
        if (!_garage.Contains(carSnapshot))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "garage.car_snapshot.not_found",
                $"There is no such car in the garage."));
        }

        _garage.Remove(carSnapshot);
        AddDomainEvent(new CarRemovedFromGarageEvent(carSnapshot.CarId));
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateProfilePhoto(Guid image)
    {
        ProfilePhoto = image;
        AddDomainEvent(new UserUpdatedEvent(this));
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveProfilePhoto()
    {
        ProfilePhoto = null;
        AddDomainEvent(new UserUpdatedEvent(this));
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddAd(Guid ad)
    {
        if (_ads.Contains(ad))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "user.ad.already_exists",
                $"User already has this ad."));
        }

        _ads.Add(ad);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveAd(Guid ad)
    {
        if (!_ads.Contains(ad))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "user.ad.not_found",
                $"User doesn't have such ad."));
        }

        _ads.Remove(ad);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddBookmark(Guid ad)
    {
        if (_bookmarks.Contains(ad))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "user.bookmark.already_exists",
                $"This bookmark is already added."));
        }

        if (_ads.Contains(ad))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "user.bookmark.is_conflict",
                $"User cannot bookmark his ad."));
        }


        _bookmarks.Add(ad);
        AddDomainEvent(new AdBookmarkedEvent(ad));
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveBookmark(Guid ad)
    {
        if (!_bookmarks.Contains(ad))
        {
            return UnitResult.Failure<Error>(Error.Domain(
                "user.bookmark.not_found",
                $"This bookmark is not found."));
        }

        _bookmarks.Remove(ad);
        AddDomainEvent(new AdUnbookmarkedEvent(ad));
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> AddSearchString(string searchString)
    {
        if (_searchHistory.Any(s => s.Equals(searchString)))
        {
            _searchHistory.Remove(searchString);
        }

        _searchHistory.Add(searchString);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> ClearSearchHistory()
    {
        _searchHistory.Clear();
        return UnitResult.Success<Error>();
    }
}