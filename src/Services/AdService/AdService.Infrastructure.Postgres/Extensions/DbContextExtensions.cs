using AdService.Domain.Aggregates;
using AdService.Domain.Entities;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;
using AdService.Infrastructure.Postgres.Data.Seeding.Fakers;
using Microsoft.EntityFrameworkCore;

namespace AdService.Infrastructure.Postgres.Extensions;

public static class DbContextExtensions
{
    public static void SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) =>
            {
                if (context.Set<Ad>().Any())
                    return;

                var carSnapshots = CarSnapshotFaker.Fake(4);

                var currencies = CurrencyFaker.Fake(3);

                var money = MoneyFaker.Fake(4, currencies);

                var sellers = SellerSnapshotFaker.Fake(4);

                var locations = LocationFaker.Fake(4);

                var carOptions = CarOptionFaker.Fake(50);

                var ads = new Ad[6];


                SetAds(
                    ads,
                    carSnapshots,
                    locations,
                    money,
                    sellers,
                    carOptions,
                    out IReadOnlyList<Comment> comments);

                context.Set<CarOption>().AddRange(carOptions);
                context.Set<Comment>().AddRange(comments);
                context.Set<Ad>().AddRange(ads);
                context.SaveChanges();
            })
            .UseAsyncSeeding(async (context, _, ct) =>
            {
                if (await context.Set<Ad>().AnyAsync(ct))
                    return;

                var carSnapshots = CarSnapshotFaker.Fake(4);

                var currencies = CurrencyFaker.Fake(3);

                var money = MoneyFaker.Fake(4, currencies);

                var sellers = SellerSnapshotFaker.Fake(6);

                var locations = LocationFaker.Fake(4);

                var carOptions = CarOptionFaker.Fake(50);

                var ads = new Ad[6];


                SetAds(
                    ads,
                    carSnapshots,
                    locations,
                    money,
                    sellers,
                    carOptions,
                    out IReadOnlyList<Comment> comments);

                await context.Set<CarOption>().AddRangeAsync(carOptions, ct);
                await context.Set<Comment>().AddRangeAsync(comments, ct);
                await context.Set<Ad>().AddRangeAsync(ads, ct);
                await context.SaveChangesAsync(ct);
            });
    }

    private static void SetAds(
        Ad[] ads,
        CarSnapshot[] carSnapshots,
        Location[] locations,
        Money[] money,
        SellerSnapshot[] sellers,
        CarOption[] carOptions,
        out IReadOnlyList<Comment> comments)
    {
        for (int i = 0; i < ads.Length; i++)
        {
            ads[i] = Ad.Create(sellers[i]).Value;
        }

        ads[1].Update("Ad 1 gfdgsd");
        ads[2].Update(
            title: "Ad 2 gsdfgsd",
            car: carSnapshots[0],
            price: money[0],
            location: locations[0]);

        ads[2].AddImages(new List<Guid> { Guid.CreateVersion7(), Guid.CreateVersion7() });

        ads[2].Submit();

        ads[3].Update(
            title: "Ad 3 sdgdsgsdf",
            car: carSnapshots[1],
            price: money[1],
            location: locations[1]);

        ads[3].AddImages(new List<Guid> { Guid.CreateVersion7(), Guid.CreateVersion7() });
        ads[3].Submit();

        ads[3].Deny(ModerationResult.Of(Guid.NewGuid(), DateTime.UtcNow, DenyReason.COPYRIGHT_VIOLATION, "gsdfg")
            .Value);


        ads[4].Update(
            title: "Ad 4 sdfgsdgf",
            car: carSnapshots[2],
            price: money[2],
            location: locations[2]);

        ads[4].AddImages(new List<Guid> { Guid.NewGuid(), Guid.NewGuid() });
        ads[4].Submit();

        ads[4].Publish(TimeSpan.FromDays(5), ModerationResult.Of(Guid.CreateVersion7(), DateTime.UtcNow).Value);

        ads[4].AddComment(Comment.Create("Comment 1", ads[4].Id).Value);
        ads[4].AddComment(Comment.Create("Comment 1", ads[4].Id).Value);
        comments = ads[4].Comments;

        ads[5].Update(
            title: "Ad 5 sdgsdgs",
            car: carSnapshots[3],
            price: money[3],
            location: locations[3]);

        ads[5].AddImages(new List<Guid> { Guid.CreateVersion7(), Guid.CreateVersion7() });
        ads[5].AddCarOptions(new List<CarOption> { carOptions[0], carOptions[1], carOptions[2], carOptions[3] });

        ads[5].Submit();

        ads[5].Publish(TimeSpan.FromMicroseconds(5), ModerationResult.Of(Guid.CreateVersion7(), DateTime.UtcNow).Value);

        ads[5].Delete();
    }
}