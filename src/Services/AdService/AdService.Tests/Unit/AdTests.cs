using AdService.Domain.Aggregates;
using AdService.Domain.Enums;
using AdService.Domain.Events;
using AdService.Domain.ValueObjects;
using BuildingBlocks.Errors;
using FluentAssertions;
using Xunit;

namespace AdService.Tests.Unit;

public class AdTests
{
    [Fact]
    public void Create_ReturnsAdWithStatusDraftAndHasAdCreatedEvent()
    {
        var adResult = Ad.Create(Guid.NewGuid());

        adResult.Value.Status.Should().Be(AdStatus.DRAFT);
        adResult.Value.DomainEvents.Should().Contain(new AdCreatedEvent(adResult.Value));
    }

    [Fact]
    public void Update_ExpectUpdatedAdAndAdUpdatedEventWhenStatusDraft()
    {
        var adResult = Ad.Create(Guid.NewGuid());

        var ad = adResult.Value;

        ad.Update(title: "Lorem ipsum");

        ad.Title.Should().Be("Lorem ipsum");
        ad.Description.Should().Be(null);
        ad.DomainEvents.Should().Contain(new AdUpdatedEvent(ad));
    }

    [Fact]
    public void Update_ExpectConflictErrorWhenTitleIsConflict()
    {
        var adResult = Ad.Create(Guid.NewGuid());

        var ad = adResult.Value;

        var result = ad.Update(title: "Lorem");

        result.Error.Type.Should().Be(ErrorType.CONFLICT);
        result.IsFailure.Should().Be(true);
        ad.DomainEvents.Should().NotContain(new AdUpdatedEvent(ad));
    }

    [Fact]
    public void Publish_ExpectConflictErrorWhenStatusIsConflict()
    {
        var adResult = Ad.Create(Guid.NewGuid());

        var ad = adResult.Value;

        ad.Update(title: "Lorem ipsum");

        var result = ad.Publish(TimeSpan.FromHours(7), GetSuccessfulModerationResult());

        result.Error.Type.Should().Be(ErrorType.CONFLICT);
        result.IsFailure.Should().Be(true);
        ad.DomainEvents.Should().NotContain(new AdPublishedEvent(ad));
    }

    [Fact]
    public void Submit_ExpectConflictErrorWhenPropertiesAreConflict()
    {
        var ad = Ad.Create(Guid.NewGuid()).Value;

        ad.Update(title: "Lorem ipsum");

        var result = ad.Submit();

        result.IsFailure.Should().Be(true);
        result.Error.Type.Should().Be(ErrorType.CONFLICT);
        ad.DomainEvents.Should().NotContain(new AdSubmittedEvent(ad));
    }

    [Fact]
    public void Submit_ExpectAdStatusPendingAndAdSubmittedEvent()
    {
        var ad = Ad.Create(Guid.NewGuid()).Value;

        ad.Update(
            title: "Lorem ipsum",
            car: GetCarSnapshot(),
            price: GetMoney(),
            seller: GetSellerSnapshot(Guid.NewGuid()),
            location: GetLocation());

        ad.AddImages(new List<Guid>([Guid.NewGuid()]));
        var result = ad.Submit();

        result.IsSuccess.Should().Be(true);
        ad.Status.Should().Be(AdStatus.PENDING);
        ad.DomainEvents.Should().Contain(new AdSubmittedEvent(ad));
    }

    [Fact]
    public void Deny_ExpectAdStatusDeniedAndAdDeniedEvent()
    {
        var ad = GetAd();
        ad.Update(
            title: "Lorem ipsum",
            car: GetCarSnapshot(),
            price: GetMoney(),
            seller: GetSellerSnapshot(ad.SellerId),
            location: GetLocation());

        ad.AddImages(GetImages());
        ad.Submit();

        var result = ad.Deny(GetFailedModerationResult());

        result.IsSuccess.Should().Be(true);
        ad.Status.Should().Be(AdStatus.DENIED);
        ad.DomainEvents.Should().Contain(new AdDeniedEvent(ad));
    }

    [Fact]
    public void Publish_ExpectAdStatusPublishedAndAdPublishedEvent()
    {
        var ad = GetAd();

        ad.Update(
            title: "Lorem ipsum",
            car: GetCarSnapshot(),
            price: GetMoney(),
            seller: GetSellerSnapshot(ad.SellerId),
            location: GetLocation());

        ad.AddImages(GetImages());
        ad.Submit();

        var result = ad.Publish(TimeSpan.FromHours(7), GetSuccessfulModerationResult());

        result.IsSuccess.Should().Be(true);
        ad.Status.Should().Be(AdStatus.PUBLISHED);
        ad.DomainEvents.Should().Contain(new AdPublishedEvent(ad));
    }

    [Fact]
    public void Publish_ExpectConflictErrorWhenModerationResultNotAccepted()
    {
        var ad = GetAd();

        ad.Update(
            title: "Lorem ipsum",
            car: GetCarSnapshot(),
            price: GetMoney(),
            seller: GetSellerSnapshot(ad.SellerId),
            location: GetLocation());

        ad.AddImages(GetImages());
        ad.Submit();

        var result = ad.Publish(TimeSpan.FromHours(7), GetFailedModerationResult());

        result.IsFailure.Should().Be(true);
        ad.Status.Should().NotBe(AdStatus.PUBLISHED);
        ad.DomainEvents.Should().NotContain(new AdPublishedEvent(ad));
    }

    [Fact]
    public void Expire_ExpectConflictErrorWhenAdNotExpiredYet()
    {
        var ad = GetAd();

        ad.Update(
            title: "Lorem ipsum",
            car: GetCarSnapshot(),
            price: GetMoney(),
            seller: GetSellerSnapshot(ad.SellerId),
            location: GetLocation());

        ad.AddImages(GetImages());
        ad.Submit();

        ad.Publish(TimeSpan.FromHours(7), GetSuccessfulModerationResult());

        var result = ad.Expire();

        result.IsFailure.Should().Be(true);
        ad.Status.Should().NotBe(AdStatus.EXPIRED);
        ad.DomainEvents.Should().NotContain(new AdExpiredEvent(ad));
    }

    [Fact]
    public void Pause_ExpectAdStatusPausedAndAdPausedEvent()
    {
        var ad = GetAd();

        ad.Update(
            title: "Lorem ipsum",
            car: GetCarSnapshot(),
            price: GetMoney(),
            seller: GetSellerSnapshot(ad.SellerId),
            location: GetLocation());

        ad.AddImages(GetImages());
        ad.Submit();

        ad.Publish(TimeSpan.FromHours(7), GetSuccessfulModerationResult());

        var result = ad.Pause();

        result.IsSuccess.Should().Be(true);
        ad.Status.Should().Be(AdStatus.PAUSED);
        ad.DomainEvents.Should().Contain(new AdPausedEvent(ad));
    }


    private CarSnapshot GetCarSnapshot() =>
        CarSnapshot.Of(
            "fsdf",
            "fadf",
            1995,
            "gsfdg",
            null,
            1243,
            "green",
            150,
            AutoDriveType.AWD,
            TransmissionType.AUTOMATIC,
            FuelType.DIESEL).Value;

    private Money GetMoney() => Money.Of(Currency.Of("BYN").Value, 1200).Value;

    private SellerSnapshot GetSellerSnapshot(Guid sellerId) =>
        SellerSnapshot.Of("gsd", DateTime.UtcNow, sellerId).Value;

    private Location GetLocation() => Location.Of("fdasgs", "fdgsgs").Value;

    private Ad GetAd() => Ad.Create(Guid.NewGuid()).Value;

    private ModerationResult GetFailedModerationResult() =>
        ModerationResult.Of(Guid.NewGuid(), DateTime.UtcNow, DenyReason.FRAUD_SUSPICION).Value;

    private ModerationResult GetSuccessfulModerationResult() =>
        ModerationResult.Of(Guid.NewGuid(), DateTime.UtcNow).Value;

    private IList<Guid> GetImages() => new List<Guid>([Guid.NewGuid()]);
}