using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProfileService.Application.Abstractions.IdentityProvider;
using ProfileService.Application.Commands.DeleteMyProfile;
using ProfileService.Application.Commands.DeleteProfileById;
using ProfileService.Tests.Support;
using Xunit;

namespace ProfileService.Tests.Application;

public class DeleteProfileCommandTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_DeletesOnlySelectedProfileAndItsKeycloakAccount(bool byKeycloakId)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        profile.AddAdSnapshot(TestProfiles.CreateAd("02efd0a9-16fd-4fd9-b298-83aab1127b90"));
        var otherProfile = TestProfiles.Create("other");
        otherProfile.AddAdSnapshot(TestProfiles.CreateAd("other-ad"));
        await database.SeedAsync(profile, otherProfile);
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        keycloak.Setup(x => x.DeleteUserAsync(profile.KeycloakId, cancellation.Token)).Returns(Task.CompletedTask);

        var result = await DeleteAsync(
            context, keycloak.Object, byKeycloakId, byKeycloakId ? profile.KeycloakId : profile.Id, cancellation.Token);

        result.IsSuccess.Should().BeTrue();
        context.SaveChangesCalls.Should().Be(1);
        (await database.FindProfileAsync(profile.Id)).Should().BeNull();
        (await database.FindProfileAsync(otherProfile.Id)).Should().BeEquivalentTo(
            otherProfile, options => options.Excluding(x => x.DomainEvents));
        keycloak.Verify(x => x.DeleteUserAsync(profile.KeycloakId, cancellation.Token), Times.Once);
        keycloak.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task Handle_WhenProfileMissingOrGivenWrongIdKind_ReturnsNotFoundWithoutSideEffects(
        bool byKeycloakId, bool useWrongIdKind)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        var id = useWrongIdKind ? byKeycloakId ? profile.Id : profile.KeycloakId : Guid.NewGuid();

        var result = await DeleteAsync(context, keycloak.Object, byKeycloakId, id, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().ContainSingle().Which.Type.Should().Be(ErrorType.NOT_FOUND);
        context.SaveChangesCalls.Should().Be(0);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        keycloak.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_WhenKeycloakFails_PropagatesErrorWithoutDeletingProfiles(bool byKeycloakId)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        profile.AddAdSnapshot(TestProfiles.CreateAd("02efd0a9-16fd-4fd9-b298-83aab1127b90"));
        var otherProfile = TestProfiles.Create("other");
        await database.SeedAsync(profile, otherProfile);
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        keycloak.Setup(x => x.DeleteUserAsync(profile.KeycloakId, cancellation.Token))
            .ThrowsAsync(new HttpRequestException("Keycloak unavailable"));

        var action = () => DeleteAsync(
            context, keycloak.Object, byKeycloakId, byKeycloakId ? profile.KeycloakId : profile.Id, cancellation.Token);

        await action.Should().ThrowAsync<HttpRequestException>().WithMessage("Keycloak unavailable");
        context.SaveChangesCalls.Should().Be(0);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        (await database.FindProfileAsync(otherProfile.Id)).Should().BeEquivalentTo(
            otherProfile, options => options.Excluding(x => x.DomainEvents));
        keycloak.Verify(x => x.DeleteUserAsync(profile.KeycloakId, cancellation.Token), Times.Once);
        keycloak.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_WhenSavingFails_PropagatesErrorWithoutDeletingProfileFromDatabase(bool byKeycloakId)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        context.SaveFailure = new DbUpdateException("Database unavailable");
        var keycloak = new Mock<IAdminKeycloakService>(MockBehavior.Strict);
        keycloak.Setup(x => x.DeleteUserAsync(profile.KeycloakId, CancellationToken.None)).Returns(Task.CompletedTask);

        var action = () => DeleteAsync(
            context, keycloak.Object, byKeycloakId, byKeycloakId ? profile.KeycloakId : profile.Id, CancellationToken.None);

        await action.Should().ThrowAsync<DbUpdateException>().WithMessage("Database unavailable");
        context.SaveChangesCalls.Should().Be(1);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        keycloak.Verify(x => x.DeleteUserAsync(profile.KeycloakId, CancellationToken.None), Times.Once);
        keycloak.VerifyNoOtherCalls();
    }

    private static Task<UnitResult<List<Error>>> DeleteAsync(
        TestAppDbContext context,
        IAdminKeycloakService keycloak,
        bool byKeycloakId,
        Guid id,
        CancellationToken cancellationToken) =>
        byKeycloakId
            ? new DeleteMyProfileCommandHandler(context, keycloak).Handle(new DeleteMyProfileCommand(id), cancellationToken)
            : new DeleteProfileByIdCommandHandler(context, keycloak).Handle(new DeleteProfileByIdCommand(id), cancellationToken);
}
