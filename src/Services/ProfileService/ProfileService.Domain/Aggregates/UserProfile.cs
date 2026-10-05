using System.Text.Json.Serialization;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using ProfileService.Domain.Events;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Domain.Aggregates;

public sealed class UserProfile : Aggregate<Guid>
{
    public const int MAX_NAME_LENGTH = 100;
    public const int MAX_SURNAME_LENGTH = 100;

    public const int MAX_USERNAME_LENGTH = 255;

    public const int MAX_PICTURE_LENGTH = 500;

    private readonly List<AdSnapshot> _ads = [];

    public Guid KeycloakId { get; private set; }

    public Email Email { get; private set; } = null!;

    public string Username { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string Surname { get; private set; } = null!;

    public PhoneNumber PhoneNumber { get; private set; }

    public string? Picture { get; private set; }

    public IReadOnlyList<AdSnapshot> Ads => _ads.AsReadOnly();

    [JsonConstructor]
    private UserProfile()
    {
    }

    private UserProfile(
        Guid keycloakId,
        Email email,
        string username,
        string name,
        string surname,
        PhoneNumber phoneNumber,
        string picture)
    {
        KeycloakId = keycloakId;
        Email = email;
        Username = username;
        Name = name;
        Surname = surname;
        PhoneNumber = phoneNumber;
        Picture = picture;
    }

    public static Result<UserProfile, Error> Create(
        Guid keycloakId,
        Email email,
        string username,
        string name,
        string surname,
        PhoneNumber phoneNumber,
        string? picture = null)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Failure<UserProfile, Error>(Error.Validation("Username cannot be empty"));

        if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
        {
            return Result.Failure<UserProfile, Error>(
                Error.Validation($"Name must be between 1 and {MAX_NAME_LENGTH} characters"));
        }

        if (string.IsNullOrWhiteSpace(surname) || surname.Length > MAX_SURNAME_LENGTH)
        {
            return Result.Failure<UserProfile, Error>(
                Error.Validation($"Surname must be between 1 and {MAX_SURNAME_LENGTH} characters"));
        }

        var profile = new UserProfile
        {
            Id = Guid.CreateVersion7(),
            KeycloakId = keycloakId,
            Email = email,
            Username = username,
            Name = name,
            Surname = surname,
            PhoneNumber = phoneNumber,
            Picture = picture,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = username,
        };

        profile.AddDomainEvent(new UserProfileCreatedEvent(profile));
        return Result.Success<UserProfile, Error>(profile);
    }

    public Result<UserProfile, Error> UpdateProfile(
        string? name = null,
        string? surname = null,
        string? picture = null)
    {
        if (name is not null)
        {
            if (name.Length > MAX_NAME_LENGTH)
            {
                return Result.Failure<UserProfile, Error>(
                    Error.Domain("name.is_conflict", $"Name must be between 1 and {MAX_NAME_LENGTH} characters"));
            }

            Name = name;
        }

        if (surname is not null)
        {
            if (surname.Length > MAX_SURNAME_LENGTH)
            {
                return Result.Failure<UserProfile, Error>(
                    Error.Domain($"Surname must be between 1 and {MAX_SURNAME_LENGTH} characters"));
            }

            Surname = surname;
        }

        if (picture is not null)
        {
            Picture = picture;
        }

        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new UserProfileUpdatedEvent(this));
        return Result.Success<UserProfile, Error>(this);
    }

    public Result<UserProfile, Error> ReplaceProfile(
        string username,
        Email email,
        string name,
        string surname,
        PhoneNumber phoneNumber,
        string? picture)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length > MAX_USERNAME_LENGTH)
            return Result.Failure<UserProfile, Error>(Error.Validation("username", $"Username must be between 1 and {MAX_USERNAME_LENGTH} characters."));

        if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
            return Result.Failure<UserProfile, Error>(Error.Validation("name", $"Name must be between 1 and {MAX_NAME_LENGTH} characters."));

        if (string.IsNullOrWhiteSpace(surname) || surname.Length > MAX_SURNAME_LENGTH)
            return Result.Failure<UserProfile, Error>(Error.Validation("surname", $"Surname must be between 1 and {MAX_SURNAME_LENGTH} characters."));

        if (picture?.Length > MAX_PICTURE_LENGTH)
            return Result.Failure<UserProfile, Error>(Error.Validation("picture", $"Picture must not exceed {MAX_PICTURE_LENGTH} characters."));

        if (Username == username && Email == email && Name == name && Surname == surname &&
            PhoneNumber == phoneNumber && Picture == picture)
            return Result.Success<UserProfile, Error>(this);

        Username = username;
        Email = email;
        Name = name;
        Surname = surname;
        PhoneNumber = phoneNumber;
        Picture = picture;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = username;
        AddDomainEvent(new UserProfileUpdatedEvent(this));

        return Result.Success<UserProfile, Error>(this);
    }

    public Result<UserProfile, Error> AddAdSnapshot(AdSnapshot adSnapshot)
    {
        if (_ads.Any(a => a.AdId == adSnapshot.AdId))
            return Result.Failure<UserProfile, Error>(Error.Conflict("Ad snapshot already exists"));

        _ads.Add(adSnapshot);
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AdSnapshotAddedEvent(this, adSnapshot));
        return Result.Success<UserProfile, Error>(this);
    }

    public Result<UserProfile, Error> UpdateAdSnapshot(AdSnapshot adSnapshot)
    {
        var existingAd = _ads.FirstOrDefault(a => a.AdId == adSnapshot.AdId);
        if (existingAd is null)
            return Result.Failure<UserProfile, Error>(Error.NotFound("Ad snapshot not found"));

        _ads.Remove(existingAd);
        _ads.Add(adSnapshot);
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AdSnapshotUpdatedEvent(this, adSnapshot));
        return Result.Success<UserProfile, Error>(this);
    }

    public Result<UserProfile, Error> RemoveAdSnapshot(string adId)
    {
        var adSnapshot = _ads.FirstOrDefault(a => a.AdId == adId);
        if (adSnapshot is null)
            return Result.Failure<UserProfile, Error>(Error.NotFound("Ad snapshot not found"));

        _ads.Remove(adSnapshot);
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AdSnapshotRemovedEvent(this, adId));
        return Result.Success<UserProfile, Error>(this);
    }
}