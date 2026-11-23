using BuildingBlocks.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProfileService.Application.Abstractions.IdentityProvider;
using ProfileService.Application.Commands.UpdateMyProfile;
using ProfileService.Application.Dtos;
using ProfileService.Tests.Support;
using Xunit;

namespace ProfileService.Tests.Application;

public class UpdateMyProfileCommandTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_UpdatesSelectedProfileAndKeycloakWithoutChangingOtherUser(bool changeUsername)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: Guid.NewGuid());
        profile.AddAdSnapshot(TestProfiles.CreateAd("ad-1"));
        var otherProfile = TestProfiles.Create("other");
        await database.SeedAsync(profile, otherProfile);
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        var data = NewProfileData(changeUsername ? "new-seller" : profile.Username);
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        var keycloakData = new UpdateKeycloakProfileDto("Petr", "Petrov", "new@example.com", data.Username);
        keycloak.Setup(x => x.UpdateUserAsync(profile.KeycloakId, keycloakData, cancellation.Token))
            .Returns(Task.CompletedTask);
        var handler = new UpdateMyProfileCommandHandler(context, keycloak.Object);

        var result = await handler.Handle(new UpdateMyProfileCommand(profile.KeycloakId, data), cancellation.Token);

        result.IsSuccess.Should().BeTrue();
        context.SaveChangesCalls.Should().Be(1);
        var stored = await database.FindProfileAsync(profile.Id);
        stored.Should().NotBeNull();
        stored!.KeycloakId.Should().Be(profile.KeycloakId);
        stored.Username.Should().Be(data.Username);
        stored.Email.Value.Should().Be("new@example.com");
        stored.Name.Should().Be(data.Name);
        stored.Surname.Should().Be(data.Surname);
        stored.PhoneNumber.E164.Should().Be(data.PhoneNumber);
        stored.Picture.Should().Be(profile.Picture);
        stored.CreatedAt.Should().Be(profile.CreatedAt);
        stored.CreatedBy.Should().Be(profile.CreatedBy);
        stored.Ads.Should().BeEquivalentTo(profile.Ads);
        (await database.FindProfileAsync(otherProfile.Id)).Should().BeEquivalentTo(
            otherProfile, options => options.Excluding(x => x.DomainEvents));
        keycloak.Verify(x => x.UpdateUserAsync(profile.KeycloakId, keycloakData, cancellation.Token), Times.Once);
        keycloak.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_WhenUsernameBelongsToOtherUser_ReturnsConflictWithoutSavingOrCallingKeycloak()
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        var otherProfile = TestProfiles.Create("other");
        await database.SeedAsync(profile, otherProfile);
        await using var context = database.CreateContext();
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        var handler = new UpdateMyProfileCommandHandler(context, keycloak.Object);

        var result = await handler.Handle(
            new UpdateMyProfileCommand(profile.KeycloakId, NewProfileData(otherProfile.Username)), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().ContainSingle().Which.Type.Should().Be(ErrorType.CONFLICT);
        context.SaveChangesCalls.Should().Be(0);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        (await database.FindProfileAsync(otherProfile.Id)).Should().BeEquivalentTo(
            otherProfile, options => options.Excluding(x => x.DomainEvents));
        keycloak.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_WhenProfileMissingOrGivenLocalId_ReturnsNotFoundWithoutSideEffects(bool useLocalId)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        var handler = new UpdateMyProfileCommandHandler(context, keycloak.Object);

        var result = await handler.Handle(
            new UpdateMyProfileCommand(useLocalId ? profile.Id : Guid.NewGuid(), NewProfileData()), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().ContainSingle().Which.Type.Should().Be(ErrorType.NOT_FOUND);
        context.SaveChangesCalls.Should().Be(0);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        keycloak.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_WhenKeycloakFails_PropagatesErrorWithoutPersistingProfileChanges()
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        var otherProfile = TestProfiles.Create("other");
        await database.SeedAsync(profile, otherProfile);
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        var data = NewProfileData();
        var keycloakData = new UpdateKeycloakProfileDto("Petr", "Petrov", "new@example.com", data.Username);
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        keycloak.Setup(x => x.UpdateUserAsync(profile.KeycloakId, keycloakData, cancellation.Token))
            .ThrowsAsync(new HttpRequestException("Keycloak unavailable"));
        var handler = new UpdateMyProfileCommandHandler(context, keycloak.Object);

        var action = () => handler.Handle(new UpdateMyProfileCommand(profile.KeycloakId, data), cancellation.Token);

        await action.Should().ThrowAsync<HttpRequestException>().WithMessage("Keycloak unavailable");
        context.SaveChangesCalls.Should().Be(0);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        (await database.FindProfileAsync(otherProfile.Id)).Should().BeEquivalentTo(
            otherProfile, options => options.Excluding(x => x.DomainEvents));
        keycloak.Verify(x => x.UpdateUserAsync(profile.KeycloakId, keycloakData, cancellation.Token), Times.Once);
        keycloak.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_WhenSavingFails_PropagatesErrorWithoutPersistingProfileChanges()
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        context.SaveFailure = new DbUpdateException("Database unavailable");
        var data = NewProfileData();
        var keycloakData = new UpdateKeycloakProfileDto("Petr", "Petrov", "new@example.com", data.Username);
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        keycloak.Setup(x => x.UpdateUserAsync(profile.KeycloakId, keycloakData, CancellationToken.None))
            .Returns(Task.CompletedTask);
        var handler = new UpdateMyProfileCommandHandler(context, keycloak.Object);

        var action = () => handler.Handle(new UpdateMyProfileCommand(profile.KeycloakId, data), CancellationToken.None);

        await action.Should().ThrowAsync<DbUpdateException>().WithMessage("Database unavailable");
        context.SaveChangesCalls.Should().Be(1);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        keycloak.Verify(x => x.UpdateUserAsync(profile.KeycloakId, keycloakData, CancellationToken.None), Times.Once);
        keycloak.VerifyNoOtherCalls();
    }

    private static UpdateMyProfileDto NewProfileData(string username = "new-seller") =>
        new(username, "NEW@example.com", "Petr", "Petrov", "+375331234567");
}
