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

                var transmissionTypes = TransmissionTypeSnapshotFaker.Fake(3);

                var bodyTypes = BodyTypeSnapshotFaker.Fake(4);

                var driveTypes = AutoDriveTypeSnapshotFaker.Fake(3);

                var fuelTypes = FuelTypeSnapshotFaker.Fake(3);

                var brands = BrandSnapshotFaker.Fake(4);

                var models = ModelSnapshotFaker.Fake(4, brands);

                var generations = GenerationSnapshotFaker.Fake(4, models);

                var engines = EngineSnapshotFaker.Fake(4, generations, fuelTypes);

                var carSnapshots = CarSnapshotFaker.Fake(4, brands, models, generations, engines, transmissionTypes, driveTypes, bodyTypes);

                var currencies = CurrencyFaker.Fake(3);

                var money = MoneyFaker.Fake(4, currencies);

                var phoneNumbers = PhoneNumberFaker.Fake(4);

                var sellers = SellerSnapshotFaker.Fake(6, phoneNumbers);

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
                    out List<Comment> comments);

                context.Set<CarOption>().AddRange(carOptions);
                context.Set<Comment>().AddRange(comments);
                context.Set<Ad>().AddRange(ads);
                context.SaveChanges();
            })
            .UseAsyncSeeding(async (context, _, ct) =>
            {
                if (await context.Set<Ad>().AnyAsync(ct))
                    return;

                var transmissionTypes = TransmissionTypeSnapshotFaker.Fake(3);

                var bodyTypes = BodyTypeSnapshotFaker.Fake(4);

                var driveTypes = AutoDriveTypeSnapshotFaker.Fake(3);

                var fuelTypes = FuelTypeSnapshotFaker.Fake(3);

                var brands = BrandSnapshotFaker.Fake(4);

                var models = ModelSnapshotFaker.Fake(4, brands);

                var generations = GenerationSnapshotFaker.Fake(4, models);

                var engines = EngineSnapshotFaker.Fake(4, generations, fuelTypes);

                var carSnapshots = CarSnapshotFaker.Fake(4, brands, models, generations, engines, transmissionTypes, driveTypes, bodyTypes);

                var currencies = CurrencyFaker.Fake(3);

                var money = MoneyFaker.Fake(4, currencies);

                var phoneNumbers = PhoneNumberFaker.Fake(4);

                var sellers = SellerSnapshotFaker.Fake(6, phoneNumbers);

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
                    out List<Comment> comments);

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
        out List<Comment> comments)
    {
        comments = new List<Comment>();
        for (int i = 0; i < ads.Length; i++)
        {
            ads[i] = Ad.Create(sellers[i]).Value;
        }

        ads[1].UpdateCar(carSnapshots[0]);
        ads[1].Update("Ad 1 gfdgsd");
        ads[2].Update(
            title: "Ad 2 gsdfgsd",
            price: money[0],
            location: locations[0]);

        ads[2].AddImages(new List<Guid> { Guid.CreateVersion7(), Guid.CreateVersion7() });

        ads[2].Submit();

        ads[3].UpdateCar(carSnapshots[1]);
        ads[3].Update(
            title: "Ad 3 sdgdsgsdf",
            price: money[1],
            location: locations[1]);

        ads[3].AddImages(new List<Guid> { Guid.CreateVersion7(), Guid.CreateVersion7() });
        ads[3].Submit();

        ads[3].Deny(ModerationResult.Of(Guid.NewGuid(), DateTime.UtcNow, DenyReason.COPYRIGHT_VIOLATION, "gsdfg")
            .Value);


        ads[4].UpdateCar(carSnapshots[2]);
        ads[4].Update(
            title: "Ad 4 sdfgsdgf",
            price: money[2],
            location: locations[2]);

        ads[4].AddImages(new List<Guid> { Guid.NewGuid(), Guid.NewGuid() });
        ads[4].Submit();

        ads[4].Publish(TimeSpan.FromDays(5), ModerationResult.Of(Guid.CreateVersion7(), DateTime.UtcNow).Value);

        ads[4].AddComment(Comment.Create("Comment 1", ads[4].Id).Value);
        comments.Add(ads[4].Comment!);

        ads[5].UpdateCar(carSnapshots[3]);
        ads[5].Update(
            title: "Ad 5 sdgsdgs",
            price: money[3],
            location: locations[3]);

        ads[5].AddImages(new List<Guid> { Guid.CreateVersion7(), Guid.CreateVersion7() });
        ads[5].AddCarOptions(new List<CarOption> { carOptions[0], carOptions[1], carOptions[2], carOptions[3] });

        ads[5].Submit();

        ads[5].Publish(TimeSpan.FromMicroseconds(5), ModerationResult.Of(Guid.CreateVersion7(), DateTime.UtcNow).Value);

        ads[5].Delete();
    }
}