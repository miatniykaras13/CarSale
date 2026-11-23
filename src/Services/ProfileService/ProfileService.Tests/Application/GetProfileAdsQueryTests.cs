using BuildingBlocks.Errors;
using FluentAssertions;
using ProfileService.Application.Dtos;
using ProfileService.Application.Queries.GetMyAds;
using ProfileService.Application.Queries.GetProfileAdsById;
using ProfileService.Tests.Support;
using Xunit;

namespace ProfileService.Tests.Application;

public class GetProfileAdsQueryTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetAds_ReturnsOnlySelectedUsersAdsWithNestedAndOptionalData(bool byKeycloakId)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        profile.AddAdSnapshot(TestProfiles.CreateAd("ad-1"));
        profile.AddAdSnapshot(TestProfiles.CreateAd("ad-2", withDetails: false));
        var otherProfile = TestProfiles.Create("other");
        otherProfile.AddAdSnapshot(TestProfiles.CreateAd("other-ad"));
        await database.SeedAsync(profile, otherProfile);
        await using var context = database.CreateContext();

        var result = byKeycloakId
            ? await new GetMyAdsQueryHandler(context).Handle(new GetMyAdsQuery(profile.KeycloakId), CancellationToken.None)
            : await new GetProfileAdsByIdQueryHandler(context).Handle(new GetProfileAdsByIdQuery(profile.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new[]
        {
            new AdSnapshotDto(
                "ad-1",
                "PUBLISHED",
                "Car for sale",
                new CarSnapshotDto("BMW", "3 Series", "F30", 2015, "RWD", "Automatic", 2.0, "Petrol", "Sedan"),
                new MoneyDto(15000, new CurrencyDto("USD"))),
            new AdSnapshotDto("ad-2", "DRAFT", null, null, null),
        });
        context.SaveChangesCalls.Should().Be(0);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetAds_WhenProfileHasNoAds_ReturnsSuccessfulEmptyList(bool byKeycloakId)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();

        var result = byKeycloakId
            ? await new GetMyAdsQueryHandler(context).Handle(new GetMyAdsQuery(profile.KeycloakId), CancellationToken.None)
            : await new GetProfileAdsByIdQueryHandler(context).Handle(new GetProfileAdsByIdQuery(profile.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        context.SaveChangesCalls.Should().Be(0);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task GetAds_WhenProfileMissingOrGivenWrongIdKind_ReturnsNotFound(bool byKeycloakId, bool useWrongIdKind)
    {
        using var database = new ProfileTestDatabase();
        var profile = TestProfiles.Create();
        profile.AddAdSnapshot(TestProfiles.CreateAd("ad-1"));
        await database.SeedAsync(profile);
        await using var context = database.CreateContext();
        var id = useWrongIdKind ? byKeycloakId ? profile.Id : profile.KeycloakId : Guid.NewGuid();

        var result = byKeycloakId
            ? await new GetMyAdsQueryHandler(context).Handle(new GetMyAdsQuery(id), CancellationToken.None)
            : await new GetProfileAdsByIdQueryHandler(context).Handle(new GetProfileAdsByIdQuery(id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().ContainSingle().Which.Type.Should().Be(ErrorType.NOT_FOUND);
        context.SaveChangesCalls.Should().Be(0);
    }
}
