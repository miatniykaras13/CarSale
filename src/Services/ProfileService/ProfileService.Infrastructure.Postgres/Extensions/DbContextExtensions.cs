using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Aggregates;
using ProfileService.Domain.ValueObjects;

namespace ProfileService.Infrastructure.Postgres.Extensions;

public static class DbContextExtensions
{
    private const string VLAD_USERNAME = "vlad";

    private static readonly Guid _vladKeycloakId = Guid.Parse("b715afc6-a42e-4e90-91f8-8e735d735ef9");

    public static void SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) =>
            {
                var profiles = context.Set<UserProfile>();

                if (profiles.Any(profile => profile.Username == VLAD_USERNAME || profile.KeycloakId == _vladKeycloakId))
                    return;

                profiles.Add(CreateVladProfile());
                context.SaveChanges();
            })
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var profiles = context.Set<UserProfile>();

                if (await profiles.AnyAsync(
                        profile => profile.Username == VLAD_USERNAME || profile.KeycloakId == _vladKeycloakId,
                        cancellationToken))
                    return;

                profiles.Add(CreateVladProfile());
                await context.SaveChangesAsync(cancellationToken);
            });
    }

    private static UserProfile CreateVladProfile()
    {
        var profile = UserProfile.Create(
            _vladKeycloakId,
            Email.Of("vlad@test.com").Value,
            VLAD_USERNAME,
            "vlad",
            "vlad",
            PhoneNumber.Of("+375291234567").Value).Value;

        profile.CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(1772611200000).UtcDateTime;
        profile.UpdatedAt = profile.CreatedAt;
        profile.UpdatedBy = VLAD_USERNAME;

        return profile;
    }
}
