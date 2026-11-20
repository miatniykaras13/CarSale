using BuildingBlocks.Errors;
using FluentAssertions;
using ProfileService.Domain.Aggregates;
using ProfileService.Domain.Events;
using ProfileService.Domain.ValueObjects;
using Xunit;

namespace ProfileService.Tests.Unit;

public class UserProfileTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Create_ReturnsProfileWithProvidedDataAndCreatedEvent(bool withPicture)
    {
        var keycloakId = Guid.NewGuid();
        var email = Email.Of("seller@example.com").Value;
        var phoneNumber = PhoneNumber.Of("+375291234567").Value;
        Guid? pictureId = withPicture ? Guid.NewGuid() : null;
        var beforeCreation = DateTime.UtcNow;

        var result = UserProfile.Create(
            keycloakId, email, "seller", "Ivan", "Ivanov", phoneNumber, pictureId);

        result.IsSuccess.Should().BeTrue();
        var profile = result.Value;
        profile.Id.Should().NotBeEmpty();
        profile.KeycloakId.Should().Be(keycloakId);
        profile.Username.Should().Be("seller");
        profile.Email.Should().Be(email);
        profile.Name.Should().Be("Ivan");
        profile.Surname.Should().Be("Ivanov");
        profile.PhoneNumber.Should().Be(phoneNumber);
        profile.Picture.Should().Be(pictureId);
        profile.Ads.Should().BeEmpty();
        profile.CreatedBy.Should().Be("seller");
        profile.CreatedAt.Should().BeOnOrAfter(beforeCreation).And.BeOnOrBefore(DateTime.UtcNow);
        profile.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserProfileCreatedEvent>()
            .Which.UserProfile.Should().BeSameAs(profile);
    }

    [Fact]
    public void Create_GeneratesDifferentIdsForDifferentProfiles()
    {
        var first = CreateProfile();
        var second = CreateProfile();

        first.Id.Should().NotBe(second.Id);
    }

    [Theory]
    [InlineData("Petr", null, "Petr", "Ivanov")]
    [InlineData(null, "Petrov", "Ivan", "Petrov")]
    [InlineData("Petr", "Petrov", "Petr", "Petrov")]
    public void UpdateProfile_ChangesProvidedFieldsAndRaisesUpdatedEvent(
        string? name, string? surname, string expectedName, string expectedSurname)
    {
        var profile = CreateProfile();
        var ad = CreateAdSnapshot("ad-1");
        profile.AddAdSnapshot(ad);
        var originalIdentity = (profile.Id, profile.KeycloakId, profile.Username, profile.Email);
        var originalContacts = (profile.PhoneNumber, profile.Picture);
        var originalCreation = (profile.CreatedAt, profile.CreatedBy);
        PrepareForUpdate(profile);
        var beforeUpdate = DateTime.UtcNow;

        var result = profile.UpdateProfile(name, surname);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(profile);
        profile.Name.Should().Be(expectedName);
        profile.Surname.Should().Be(expectedSurname);
        (profile.Id, profile.KeycloakId, profile.Username, profile.Email).Should().Be(originalIdentity);
        (profile.PhoneNumber, profile.Picture).Should().Be(originalContacts);
        (profile.CreatedAt, profile.CreatedBy).Should().Be(originalCreation);
        profile.Ads.Should().ContainSingle().Which.Should().Be(ad);
        AssertUpdated(profile, beforeUpdate);
    }

    [Fact]
    public void ReplaceProfile_ReplacesEditableFieldsAndPreservesIdentityAndOtherData()
    {
        var profile = CreateProfile();
        var ad = CreateAdSnapshot("ad-1");
        profile.AddAdSnapshot(ad);
        var originalIdentity = (profile.Id, profile.KeycloakId);
        var originalCreation = (profile.CreatedAt, profile.CreatedBy);
        var originalPicture = profile.Picture;
        var email = Email.Of("new@example.com").Value;
        var phoneNumber = PhoneNumber.Of("+375331234567").Value;
        PrepareForUpdate(profile);
        var beforeUpdate = DateTime.UtcNow;

        var result = profile.ReplaceProfile("new-seller", email, "Petr", "Petrov", phoneNumber);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(profile);
        profile.Username.Should().Be("new-seller");
        profile.Email.Should().Be(email);
        profile.Name.Should().Be("Petr");
        profile.Surname.Should().Be("Petrov");
        profile.PhoneNumber.Should().Be(phoneNumber);
        profile.UpdatedBy.Should().Be("new-seller");
        (profile.Id, profile.KeycloakId).Should().Be(originalIdentity);
        (profile.CreatedAt, profile.CreatedBy).Should().Be(originalCreation);
        profile.Picture.Should().Be(originalPicture);
        profile.Ads.Should().ContainSingle().Which.Should().Be(ad);
        AssertUpdated(profile, beforeUpdate);
    }

    [Theory]
    [InlineData("new-seller", "seller@example.com", "Ivan", "Ivanov", "+375291234567")]
    [InlineData("seller", "new@example.com", "Ivan", "Ivanov", "+375291234567")]
    [InlineData("seller", "seller@example.com", "Petr", "Ivanov", "+375291234567")]
    [InlineData("seller", "seller@example.com", "Ivan", "Petrov", "+375291234567")]
    [InlineData("seller", "seller@example.com", "Ivan", "Ivanov", "+375331234567")]
    public void ReplaceProfile_RaisesUpdatedEventWhenAnyEditableFieldChanges(
        string username, string email, string name, string surname, string phoneNumber)
    {
        var profile = CreateProfile();
        PrepareForUpdate(profile);
        var beforeUpdate = DateTime.UtcNow;

        var result = profile.ReplaceProfile(
            username, Email.Of(email).Value, name, surname, PhoneNumber.Of(phoneNumber).Value);

        result.IsSuccess.Should().BeTrue();
        profile.Username.Should().Be(username);
        profile.Email.Value.Should().Be(email);
        profile.Name.Should().Be(name);
        profile.Surname.Should().Be(surname);
        profile.PhoneNumber.E164.Should().Be(phoneNumber);
        profile.UpdatedBy.Should().Be(username);
        AssertUpdated(profile, beforeUpdate);
    }

    [Fact]
    public void ReplaceProfile_WithEqualValues_DoesNotChangeAuditDataOrRaiseEvent()
    {
        var profile = CreateProfile();
        PrepareForUpdate(profile);
        var originalAudit = (profile.UpdatedAt, profile.UpdatedBy);

        var result = profile.ReplaceProfile(
            profile.Username,
            Email.Of(profile.Email.Value).Value,
            profile.Name,
            profile.Surname,
            PhoneNumber.Of(profile.PhoneNumber.E164).Value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(profile);
        (profile.UpdatedAt, profile.UpdatedBy).Should().Be(originalAudit);
        profile.DomainEvents.Should().BeEmpty();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void UpdatePicture_SetsPictureAndRaisesUpdatedEvent(bool withExistingPicture)
    {
        var profile = CreateProfile(withPicture: withExistingPicture);
        var originalData = (
            profile.Id, profile.KeycloakId, profile.Username, profile.Email,
            profile.Name, profile.Surname, profile.PhoneNumber, profile.CreatedAt, profile.CreatedBy);
        var pictureId = Guid.NewGuid();
        PrepareForUpdate(profile);
        var beforeUpdate = DateTime.UtcNow;

        var result = profile.UpdatePicture(pictureId);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(profile);
        profile.Picture.Should().Be(pictureId);
        (profile.Id, profile.KeycloakId, profile.Username, profile.Email,
            profile.Name, profile.Surname, profile.PhoneNumber, profile.CreatedAt, profile.CreatedBy)
            .Should().Be(originalData);
        AssertUpdated(profile, beforeUpdate);
    }

    [Fact]
    public void UpdatePicture_WithSamePicture_DoesNotChangeAuditDataOrRaiseEvent()
    {
        var profile = CreateProfile();
        var pictureId = profile.Picture!.Value;
        PrepareForUpdate(profile);
        var originalAudit = (profile.UpdatedAt, profile.UpdatedBy);

        var result = profile.UpdatePicture(pictureId);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(profile);
        profile.Picture.Should().Be(pictureId);
        (profile.UpdatedAt, profile.UpdatedBy).Should().Be(originalAudit);
        profile.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void AddAdSnapshot_AddsAdWithoutChangingExistingAdsAndRaisesAddedEvent()
    {
        var profile = CreateProfile();
        var existingAd = CreateAdSnapshot("ad-1");
        profile.AddAdSnapshot(existingAd);
        var newAd = CreateAdSnapshot("ad-2");
        PrepareForUpdate(profile);
        var beforeUpdate = DateTime.UtcNow;

        var result = profile.AddAdSnapshot(newAd);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(profile);
        profile.Ads.Should().BeEquivalentTo(new[] { existingAd, newAd });
        profile.UpdatedAt.Should().BeOnOrAfter(beforeUpdate).And.BeOnOrBefore(DateTime.UtcNow);
        var domainEvent = profile.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AdSnapshotAddedEvent>().Subject;
        domainEvent.UserProfile.Should().BeSameAs(profile);
        domainEvent.AdSnapshot.Should().Be(newAd);
    }

    [Fact]
    public void AddAdSnapshot_WithExistingAdId_ReturnsConflictWithoutChangingStateOrEvents()
    {
        var profile = CreateProfile();
        var existingAd = CreateAdSnapshot("ad-1");
        profile.AddAdSnapshot(existingAd);
        var originalTimestamp = profile.UpdatedAt;
        var originalEvents = profile.DomainEvents.ToArray();
        var duplicateAd = CreateAdSnapshot("ad-1", "PUBLISHED", "Different title");

        var result = profile.AddAdSnapshot(duplicateAd);

        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.CONFLICT);
        profile.Ads.Should().ContainSingle().Which.Should().Be(existingAd);
        profile.UpdatedAt.Should().Be(originalTimestamp);
        profile.DomainEvents.Should().Equal(originalEvents);
    }

    [Fact]
    public void UpdateAdSnapshot_ReplacesMatchingAdAndRaisesUpdatedEvent()
    {
        var profile = CreateProfile();
        var originalAd = CreateAdSnapshot("ad-1");
        var otherAd = CreateAdSnapshot("ad-2");
        profile.AddAdSnapshot(originalAd);
        profile.AddAdSnapshot(otherAd);
        var updatedAd = CreateAdSnapshot("ad-1", "PUBLISHED", "Updated title");
        PrepareForUpdate(profile);
        var beforeUpdate = DateTime.UtcNow;

        var result = profile.UpdateAdSnapshot(updatedAd);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(profile);
        profile.Ads.Should().BeEquivalentTo(new[] { updatedAd, otherAd });
        profile.UpdatedAt.Should().BeOnOrAfter(beforeUpdate).And.BeOnOrBefore(DateTime.UtcNow);
        var domainEvent = profile.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AdSnapshotUpdatedEvent>().Subject;
        domainEvent.UserProfile.Should().BeSameAs(profile);
        domainEvent.AdSnapshot.Should().Be(updatedAd);
    }

    [Fact]
    public void UpdateAdSnapshot_CanReplaceOptionalFieldsWithNull()
    {
        var profile = CreateProfile();
        var car = CarSnapshot.Of(
            "BMW", "3 Series", "F30", 2015, "RWD", "Automatic", 2.0, "Petrol", "Sedan").Value;
        var price = Money.Of(Currency.Of("USD").Value, 15000).Value;
        profile.AddAdSnapshot(AdSnapshot.Of("ad-1", "PUBLISHED", "Car for sale", car, price).Value);
        PrepareForUpdate(profile);
        var updatedAd = AdSnapshot.Of("ad-1", "DRAFT", null, null, null).Value;

        var result = profile.UpdateAdSnapshot(updatedAd);

        result.IsSuccess.Should().BeTrue();
        var storedAd = profile.Ads.Should().ContainSingle().Subject;
        storedAd.Status.Should().Be("DRAFT");
        storedAd.Title.Should().BeNull();
        storedAd.Car.Should().BeNull();
        storedAd.Price.Should().BeNull();
        profile.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AdSnapshotUpdatedEvent>()
            .Which.AdSnapshot.Should().Be(updatedAd);
    }

    [Fact]
    public void UpdateAdSnapshot_WithMissingAd_ReturnsNotFoundWithoutChangingStateOrEvents()
    {
        var profile = CreateProfile();
        var existingAd = CreateAdSnapshot("ad-1");
        profile.AddAdSnapshot(existingAd);
        var originalTimestamp = profile.UpdatedAt;
        var originalEvents = profile.DomainEvents.ToArray();

        var result = profile.UpdateAdSnapshot(CreateAdSnapshot("missing-ad"));

        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NOT_FOUND);
        profile.Ads.Should().ContainSingle().Which.Should().Be(existingAd);
        profile.UpdatedAt.Should().Be(originalTimestamp);
        profile.DomainEvents.Should().Equal(originalEvents);
    }

    [Fact]
    public void RemoveAdSnapshot_RemovesMatchingAdAndRaisesRemovedEvent()
    {
        var profile = CreateProfile();
        var removedAd = CreateAdSnapshot("ad-1");
        var otherAd = CreateAdSnapshot("ad-2");
        profile.AddAdSnapshot(removedAd);
        profile.AddAdSnapshot(otherAd);
        PrepareForUpdate(profile);
        var beforeUpdate = DateTime.UtcNow;

        var result = profile.RemoveAdSnapshot(removedAd.AdId);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(profile);
        profile.Ads.Should().ContainSingle().Which.Should().Be(otherAd);
        profile.UpdatedAt.Should().BeOnOrAfter(beforeUpdate).And.BeOnOrBefore(DateTime.UtcNow);
        var domainEvent = profile.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AdSnapshotRemovedEvent>().Subject;
        domainEvent.UserProfile.Should().BeSameAs(profile);
        domainEvent.AdId.Should().Be(removedAd.AdId);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RemoveAdSnapshot_WithMissingAd_ReturnsNotFoundWithoutChangingStateOrEvents(bool withExistingAd)
    {
        var profile = CreateProfile();
        if (withExistingAd)
            profile.AddAdSnapshot(CreateAdSnapshot("ad-1"));
        var originalAds = profile.Ads.ToArray();
        var originalTimestamp = profile.UpdatedAt;
        var originalEvents = profile.DomainEvents.ToArray();

        var result = profile.RemoveAdSnapshot("missing-ad");

        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NOT_FOUND);
        profile.Ads.Should().Equal(originalAds);
        profile.UpdatedAt.Should().Be(originalTimestamp);
        profile.DomainEvents.Should().Equal(originalEvents);
    }

    [Fact]
    public void RemoveAdSnapshot_WhenLastAdIsRemoved_LeavesEmptyCollection()
    {
        var profile = CreateProfile();
        profile.AddAdSnapshot(CreateAdSnapshot("ad-1"));
        PrepareForUpdate(profile);

        var result = profile.RemoveAdSnapshot("ad-1");

        result.IsSuccess.Should().BeTrue();
        profile.Ads.Should().BeEmpty();
        profile.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AdSnapshotRemovedEvent>();
    }

    [Fact]
    public void AddAdSnapshot_AfterRemovingSameAdId_Succeeds()
    {
        var profile = CreateProfile();
        profile.AddAdSnapshot(CreateAdSnapshot("ad-1"));
        profile.RemoveAdSnapshot("ad-1");
        PrepareForUpdate(profile);
        var newAd = CreateAdSnapshot("ad-1", "PUBLISHED", "New version");

        var result = profile.AddAdSnapshot(newAd);

        result.IsSuccess.Should().BeTrue();
        profile.Ads.Should().ContainSingle().Which.Should().Be(newAd);
        profile.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AdSnapshotAddedEvent>();
    }

    private static UserProfile CreateProfile(bool withPicture = true) =>
        UserProfile.Create(
            Guid.NewGuid(),
            Email.Of("seller@example.com").Value,
            "seller",
            "Ivan",
            "Ivanov",
            PhoneNumber.Of("+375291234567").Value,
            withPicture ? Guid.NewGuid() : null).Value;

    private static AdSnapshot CreateAdSnapshot(
        string adId, string status = "DRAFT", string title = "Car for sale") =>
        AdSnapshot.Of(adId, status, title, null, null).Value;

    private static void PrepareForUpdate(UserProfile profile)
    {
        profile.ClearDomainEvents();
        profile.UpdatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        profile.UpdatedBy = "previous-editor";
    }

    private static void AssertUpdated(UserProfile profile, DateTime beforeUpdate)
    {
        profile.UpdatedAt.Should().BeOnOrAfter(beforeUpdate).And.BeOnOrBefore(DateTime.UtcNow);
        profile.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserProfileUpdatedEvent>()
            .Which.UserProfile.Should().BeSameAs(profile);
    }
}
