using BuildingBlocks.Errors;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using ProfileService.Application.Abstractions.FileStorage;
using ProfileService.Application.Dtos;
using ProfileService.Application.Options;
using ProfileService.Application.Queries.GetMyProfile;
using ProfileService.Application.Queries.GetProfileById;
using ProfileService.Application.Queries.GetShortProfile;
using ProfileService.Tests.Support;
using Xunit;

namespace ProfileService.Tests.Application;

public class GetProfileQueryTests
{
    private const int DOWNLOAD_LINK_EXPIRY_SECONDS = 123;
    private const string PICTURE_URL = "https://files.example.com/picture";

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetMyProfile_ReturnsSelectedUsersDataByKeycloakId(bool withPicture)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: withPicture ? Guid.NewGuid() : null);
        var otherProfile = TestProfiles.Create("other");
        await database.SeedAsync(profile, otherProfile);
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        var fileStorage = CreateFileStorage(profile.Picture, cancellation.Token);
        var handler = new GetMyProfileQueryHandler(context, fileStorage.Object, CreateOptions());

        var result = await handler.Handle(new GetMyProfileQuery(profile.KeycloakId), cancellation.Token);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(new MyProfileDto(
            profile.Id,
            profile.Username,
            profile.Email.Value,
            profile.Name,
            profile.Surname,
            profile.PhoneNumber.E164,
            withPicture ? PICTURE_URL : null));
        context.SaveChangesCalls.Should().Be(0);
        VerifyFileStorage(fileStorage, profile.Picture, cancellation.Token);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetProfileById_ReturnsSelectedUsersDataByLocalId(bool withPicture)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: withPicture ? Guid.NewGuid() : null);
        await database.SeedAsync(profile, TestProfiles.Create("other"));
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        var fileStorage = CreateFileStorage(profile.Picture, cancellation.Token);
        var handler = new GetProfileByIdQueryHandler(context, fileStorage.Object, CreateOptions());

        var result = await handler.Handle(new GetProfileByIdQuery(profile.Id), cancellation.Token);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(new ProfileByIdDto(
            profile.Id,
            profile.Username,
            profile.Email.Value,
            profile.Name,
            profile.Surname,
            profile.PhoneNumber.E164,
            withPicture ? PICTURE_URL : null));
        context.SaveChangesCalls.Should().Be(0);
        VerifyFileStorage(fileStorage, profile.Picture, cancellation.Token);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetShortProfile_ReturnsSelectedUsersShortProfileByLocalId(bool withPicture)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: withPicture ? Guid.NewGuid() : null);
        await database.SeedAsync(profile, TestProfiles.Create("other"));
        await using var context = database.CreateContext();
        using var cancellation = new CancellationTokenSource();
        var fileStorage = CreateFileStorage(profile.Picture, cancellation.Token);
        var handler = new GetShortProfileQueryHandler(context, fileStorage.Object, CreateOptions());

        var result = await handler.Handle(new GetShortProfileQuery(profile.Id), cancellation.Token);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(new ShortProfileDto(profile.Id, profile.Username, withPicture ? PICTURE_URL : null));
        context.SaveChangesCalls.Should().Be(0);
        VerifyFileStorage(fileStorage, profile.Picture, cancellation.Token);
    }

    [Theory]
    [InlineData(ProfileQuery.MyProfile, false)]
    [InlineData(ProfileQuery.MyProfile, true)]
    [InlineData(ProfileQuery.ById, false)]
    [InlineData(ProfileQuery.ById, true)]
    [InlineData(ProfileQuery.Short, false)]
    [InlineData(ProfileQuery.Short, true)]
    public async Task GetProfile_WhenMissingOrGivenWrongIdKind_ReturnsNotFoundWithoutCallingFileStorage(
        ProfileQuery query, bool useWrongIdKind)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create(pictureId: Guid.NewGuid());
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        var fileStorage = new Mock<IFileStorage>(MockBehavior.Strict);
        var id = useWrongIdKind
            ? query == ProfileQuery.MyProfile ? profile.Id : profile.KeycloakId
            : Guid.NewGuid();
        List<Error> errors;

        switch (query)
        {
            case ProfileQuery.MyProfile:
                var myResult = await new GetMyProfileQueryHandler(context, fileStorage.Object, CreateOptions())
                    .Handle(new GetMyProfileQuery(id), CancellationToken.None);
                myResult.IsFailure.Should().BeTrue();
                errors = myResult.Error;
                break;
            case ProfileQuery.ById:
                var fullResult = await new GetProfileByIdQueryHandler(context, fileStorage.Object, CreateOptions())
                    .Handle(new GetProfileByIdQuery(id), CancellationToken.None);
                fullResult.IsFailure.Should().BeTrue();
                errors = fullResult.Error;
                break;
            case ProfileQuery.Short:
                var shortResult = await new GetShortProfileQueryHandler(context, fileStorage.Object, CreateOptions())
                    .Handle(new GetShortProfileQuery(id), CancellationToken.None);
                shortResult.IsFailure.Should().BeTrue();
                errors = shortResult.Error;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(query));
        }

        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.NOT_FOUND);
        context.SaveChangesCalls.Should().Be(0);
        fileStorage.VerifyNoOtherCalls();
    }

    public enum ProfileQuery
    {
        MyProfile,
        ById,
        Short,
    }

    private static IOptions<FileStorageOptions> CreateOptions() =>
        Options.Create(new FileStorageOptions { DownloadLinkExpirySeconds = DOWNLOAD_LINK_EXPIRY_SECONDS });

    private static Mock<IFileStorage> CreateFileStorage(Guid? pictureId, CancellationToken cancellationToken)
    {
        var fileStorage = new Mock<IFileStorage>(MockBehavior.Strict);
        if (pictureId.HasValue)
        {
            fileStorage.Setup(x => x.GetDownloadLinkAsync(pictureId.Value, DOWNLOAD_LINK_EXPIRY_SECONDS, cancellationToken))
                .ReturnsAsync(PICTURE_URL);
        }

        return fileStorage;
    }

    private static void VerifyFileStorage(Mock<IFileStorage> fileStorage, Guid? pictureId, CancellationToken cancellationToken)
    {
        if (pictureId.HasValue)
        {
            fileStorage.Verify(
                x => x.GetDownloadLinkAsync(pictureId.Value, DOWNLOAD_LINK_EXPIRY_SECONDS, cancellationToken), Times.Once);
        }

        fileStorage.VerifyNoOtherCalls();
    }
}
