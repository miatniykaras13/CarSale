using BuildingBlocks.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using ProfileService.Application.Abstractions.FileStorage;
using ProfileService.Application.Commands.UpdateMyProfilePicture;
using ProfileService.Application.Options;
using ProfileService.Tests.Support;
using Xunit;

namespace ProfileService.Tests.Application;

public class UpdateMyProfilePictureCommandTests
{
    private const string FILE_NAME = "avatar.png";
    private const string CONTENT_TYPE = "image/png";

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_SavesSelectedUsersNewPictureBeforeRemovingOldFile(bool withOldPicture)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: withOldPicture ? Guid.NewGuid() : null);
        profile.AddAdSnapshot(TestProfiles.CreateAd("ad-1"));
        var otherProfile = TestProfiles.Create("other", Guid.NewGuid());
        await database.SeedAsync(profile, otherProfile);
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        using var stream = CreateFile();
        var newPictureId = Guid.NewGuid();
        var saveCallsAtCleanup = 0;
        Guid? storedPictureAtCleanup = null;
        var cleanupToken = CancellationToken.None;
        var storage = new Mock<IFileStorage>(MockBehavior.Strict);
        storage.Setup(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, cancellation.Token))
            .Callback(() => context.SaveChangesCalls.Should().Be(0))
            .ReturnsAsync(newPictureId);
        if (profile.Picture is { } oldPictureId)
        {
            storage.Setup(x => x.DeleteFileAsync(oldPictureId, It.IsAny<CancellationToken>()))
                .Returns(async (Guid _, CancellationToken token) =>
                {
                    saveCallsAtCleanup = context.SaveChangesCalls;
                    storedPictureAtCleanup = (await database.FindProfileAsync(profile.Id))?.Picture;
                    cleanupToken = token;
                    return true;
                });
        }

        var result = await CreateHandler(context, storage).Handle(
            new UpdateMyProfilePictureCommand(profile.KeycloakId, stream, FILE_NAME, CONTENT_TYPE), cancellation.Token);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(newPictureId);
        context.SaveChangesCalls.Should().Be(1);
        var stored = await database.FindProfileAsync(profile.Id);
        stored.Should().NotBeNull();
        stored!.Picture.Should().Be(newPictureId);
        stored.Should().BeEquivalentTo(profile, options => options
            .Excluding(x => x.Picture).Excluding(x => x.UpdatedAt).Excluding(x => x.DomainEvents));
        (await database.FindProfileAsync(otherProfile.Id)).Should().BeEquivalentTo(
            otherProfile, options => options.Excluding(x => x.DomainEvents));
        storage.Verify(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, cancellation.Token), Times.Once);
        if (profile.Picture is { } previousPictureId)
        {
            saveCallsAtCleanup.Should().Be(1);
            storedPictureAtCleanup.Should().Be(newPictureId);
            AssertCleanupToken(cleanupToken, cancellation.Token);
            storage.Verify(x => x.DeleteFileAsync(previousPictureId, It.IsAny<CancellationToken>()), Times.Once);
        }

        storage.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_WhenProfileMissingOrGivenLocalId_DoesNotUploadOrSave(bool useLocalId)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: Guid.NewGuid());
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        using var stream = CreateFile();
        var storage = new Mock<IFileStorage>(MockBehavior.Strict);

        var result = await CreateHandler(context, storage).Handle(
            new UpdateMyProfilePictureCommand(
                useLocalId ? profile.Id : Guid.NewGuid(), stream, FILE_NAME, CONTENT_TYPE), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().ContainSingle().Which.Type.Should().Be(ErrorType.NOT_FOUND);
        context.SaveChangesCalls.Should().Be(0);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        storage.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_WhenUploadFails_PreservesProfileAndOldFile()
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: Guid.NewGuid());
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        using var stream = CreateFile();
        var exception = new HttpRequestException("File service unavailable");
        var storage = new Mock<IFileStorage>(MockBehavior.Strict);
        storage.Setup(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, cancellation.Token))
            .ThrowsAsync(exception);

        var action = () => CreateHandler(context, storage).Handle(
            new UpdateMyProfilePictureCommand(profile.KeycloakId, stream, FILE_NAME, CONTENT_TYPE), cancellation.Token);

        (await action.Should().ThrowAsync<HttpRequestException>()).Which.Should().BeSameAs(exception);
        context.SaveChangesCalls.Should().Be(0);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        storage.Verify(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, cancellation.Token), Times.Once);
        storage.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(CleanupOutcome.Success)]
    [InlineData(CleanupOutcome.NotDeleted)]
    [InlineData(CleanupOutcome.Exception)]
    public async Task Handle_WhenSavingFails_CleansUpNewFileAndPreservesOriginalErrorAndOldPicture(CleanupOutcome cleanup)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: Guid.NewGuid());
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        var exception = new DbUpdateException("Database unavailable");
        context.SaveFailure = exception;
        using var cancellation = new CancellationTokenSource();
        using var stream = CreateFile();
        var newPictureId = Guid.NewGuid();
        var cleanupToken = CancellationToken.None;
        var saveCallsAtCleanup = 0;
        var storage = new Mock<IFileStorage>(MockBehavior.Strict);
        storage.Setup(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, cancellation.Token))
            .ReturnsAsync(newPictureId);
        storage.Setup(x => x.DeleteFileAsync(newPictureId, It.IsAny<CancellationToken>()))
            .Returns((Guid _, CancellationToken token) =>
            {
                saveCallsAtCleanup = context.SaveChangesCalls;
                cleanupToken = token;
                return CleanupResult(cleanup);
            });

        var action = () => CreateHandler(context, storage).Handle(
            new UpdateMyProfilePictureCommand(profile.KeycloakId, stream, FILE_NAME, CONTENT_TYPE), cancellation.Token);

        (await action.Should().ThrowAsync<DbUpdateException>()).Which.Should().BeSameAs(exception);
        context.SaveChangesCalls.Should().Be(1);
        saveCallsAtCleanup.Should().Be(1);
        AssertCleanupToken(cleanupToken, cancellation.Token);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        storage.Verify(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, cancellation.Token), Times.Once);
        storage.Verify(x => x.DeleteFileAsync(newPictureId, It.IsAny<CancellationToken>()), Times.Once);
        storage.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(CleanupOutcome.NotDeleted)]
    [InlineData(CleanupOutcome.Exception)]
    public async Task Handle_WhenOldFileCleanupFails_KeepsSuccessfulReplacement(CleanupOutcome cleanup)
    {
        using var database = new ProfileTestDatabase();
        var oldPictureId = Guid.NewGuid();
        var profile = TestProfiles.Create(pictureId: oldPictureId);
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        using var stream = CreateFile();
        var newPictureId = Guid.NewGuid();
        var storage = new Mock<IFileStorage>(MockBehavior.Strict);
        storage.Setup(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, CancellationToken.None))
            .ReturnsAsync(newPictureId);
        storage.Setup(x => x.DeleteFileAsync(oldPictureId, It.IsAny<CancellationToken>()))
            .Returns(() => CleanupResult(cleanup));

        var result = await CreateHandler(context, storage).Handle(
            new UpdateMyProfilePictureCommand(profile.KeycloakId, stream, FILE_NAME, CONTENT_TYPE), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(newPictureId);
        context.SaveChangesCalls.Should().Be(1);
        (await database.FindProfileAsync(profile.Id))!.Picture.Should().Be(newPictureId);
        storage.Verify(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, CancellationToken.None), Times.Once);
        storage.Verify(x => x.DeleteFileAsync(oldPictureId, It.IsAny<CancellationToken>()), Times.Once);
        storage.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_WhenUploadReturnsCurrentPictureId_DoesNotDeleteThatFile()
    {
        using var database = new ProfileTestDatabase();
        var pictureId = Guid.NewGuid();
        var profile = TestProfiles.Create(pictureId: pictureId);
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        using var stream = CreateFile();
        var storage = new Mock<IFileStorage>(MockBehavior.Strict);
        storage.Setup(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, CancellationToken.None))
            .ReturnsAsync(pictureId);

        var result = await CreateHandler(context, storage).Handle(
            new UpdateMyProfilePictureCommand(profile.KeycloakId, stream, FILE_NAME, CONTENT_TYPE), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pictureId);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        storage.Verify(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, CancellationToken.None), Times.Once);
        storage.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_WhenFileServiceReturnsEmptyId_DoesNotSaveOrDeleteOldFile()
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: Guid.NewGuid());
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        using var stream = CreateFile();
        var storage = new Mock<IFileStorage>(MockBehavior.Strict);
        storage.Setup(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, CancellationToken.None))
            .ReturnsAsync(Guid.Empty);

        var result = await CreateHandler(context, storage).Handle(
            new UpdateMyProfilePictureCommand(profile.KeycloakId, stream, FILE_NAME, CONTENT_TYPE), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().ContainSingle().Which.Code.Should().Be("picture.is_invalid");
        context.SaveChangesCalls.Should().Be(0);
        (await database.FindProfileAsync(profile.Id)).Should().BeEquivalentTo(
            profile, options => options.Excluding(x => x.DomainEvents));
        storage.Verify(x => x.UploadFileAsync(stream, FILE_NAME, CONTENT_TYPE, CancellationToken.None), Times.Once);
        storage.VerifyNoOtherCalls();
    }

    public enum CleanupOutcome
    {
        Success,
        NotDeleted,
        Exception,
    }

    private static MemoryStream CreateFile() => new(new byte[] { 1, 2, 3, 4 });

    private static UpdateMyProfilePictureCommandHandler CreateHandler(
        TestAppDbContext context, Mock<IFileStorage> storage) =>
        new(
            context,
            Options.Create(new FileStorageOptions()),
            storage.Object,
            NullLogger<UpdateMyProfilePictureCommandHandler>.Instance);

    private static void AssertCleanupToken(CancellationToken token, CancellationToken requestToken)
    {
        token.Should().NotBe(requestToken);
        token.CanBeCanceled.Should().BeTrue();
        token.IsCancellationRequested.Should().BeFalse();
    }

    private static Task<bool> CleanupResult(CleanupOutcome outcome) => outcome == CleanupOutcome.Exception
        ? Task.FromException<bool>(new HttpRequestException("Cleanup unavailable"))
        : Task.FromResult(outcome == CleanupOutcome.Success);
}
