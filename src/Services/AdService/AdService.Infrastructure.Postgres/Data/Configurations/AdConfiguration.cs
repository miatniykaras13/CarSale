using AdService.Domain.Aggregates;
using AdService.Domain.Entities;
using AdService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdService.Infrastructure.Postgres.Data.Configurations;

public class AdConfiguration : IEntityTypeConfiguration<Ad>
{
    public void Configure(EntityTypeBuilder<Ad> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Car, carBuilder =>
        {
            carBuilder.ToTable("CarSnapshots");

            carBuilder.Property<Guid>("AdId")
                .HasColumnName("AdId")
                .ValueGeneratedNever();

            carBuilder.HasKey("AdId");

            carBuilder.Property(x => x.Consumption).HasPrecision(18, 1);

            carBuilder.OwnsOne(x => x.Brand, brandBuilder =>
            {
                brandBuilder.Property(x => x.Id).HasColumnName("Brand_Id").ValueGeneratedNever();
                brandBuilder.Property(x => x.Name).HasColumnName("Brand_Name");
            });

            carBuilder.OwnsOne(x => x.Model, modelBuilder =>
            {
                modelBuilder.Property(x => x.Id).HasColumnName("Model_Id").ValueGeneratedNever();
                modelBuilder.Property(x => x.Name).HasColumnName("Model_Name");
                modelBuilder.Property(x => x.BrandId).HasColumnName("Model_Brand_Id");
            });

            carBuilder.OwnsOne(x => x.Generation, generationBuilder =>
            {
                generationBuilder.Property(x => x.Id).HasColumnName("Generation_Id").ValueGeneratedNever();
                generationBuilder.Property(x => x.Name).HasColumnName("Generation_Name");
                generationBuilder.Property(x => x.ModelId).HasColumnName("Generation_Model_Id");
            });

            carBuilder.OwnsOne(x => x.Engine, engineBuilder =>
            {
                engineBuilder.Property(x => x.Id).HasColumnName("Engine_Id").ValueGeneratedNever();
                engineBuilder.Property(x => x.Name).HasColumnName("Engine_Name");
                engineBuilder.Property(x => x.GenerationId).HasColumnName("Engine_Generation_Id");
                engineBuilder.Property(x => x.HorsePower).HasColumnName("Engine_HorsePower").ValueGeneratedNever();
                engineBuilder.OwnsOne(x => x.FuelType, fuelBuilder =>
                {
                    fuelBuilder.Property(x => x.Id).HasColumnName("Engine_FuelType_Id").ValueGeneratedNever();
                    fuelBuilder.Property(x => x.Name).HasColumnName("Engine_FuelType_Name");
                });
            });

            carBuilder.OwnsOne(x => x.TransmissionType, transmissionBuilder =>
            {
                transmissionBuilder.Property(x => x.Id).HasColumnName("TransmissionType_Id").ValueGeneratedNever();
                transmissionBuilder.Property(x => x.Name).HasColumnName("TransmissionType_Name");
            });

            carBuilder.OwnsOne(x => x.DriveType, driveBuilder =>
            {
                driveBuilder.Property(x => x.Id).HasColumnName("DriveType_Id").ValueGeneratedNever();
                driveBuilder.Property(x => x.Name).HasColumnName("DriveType_Name");
            });

            carBuilder.OwnsOne(x => x.BodyType, bodyBuilder =>
            {
                bodyBuilder.Property(x => x.Id).HasColumnName("BodyType_Id").ValueGeneratedNever();
                bodyBuilder.Property(x => x.Name).HasColumnName("BodyType_Name");
            });
        });

        builder.OwnsOne(x => x.Seller, sellerBuilder =>
        {
            sellerBuilder.ToTable("SellerSnapshots");

            sellerBuilder.Property(s => s.SellerId)
                .ValueGeneratedNever();

            sellerBuilder.Property<Guid>("AdId")
                .HasColumnName("AdId")
                .ValueGeneratedNever();

            sellerBuilder.HasKey("AdId", nameof(SellerSnapshot.SellerId));

            sellerBuilder.Property(x => x.DisplayName).HasMaxLength(SellerSnapshot.MAX_NAME_LENGTH);

            sellerBuilder.OwnsOne(x => x.PhoneNumber);
        });

        builder
            .HasMany(a => a.CarOptions)
            .WithMany();

        builder
            .HasOne(a => a.Comment)
            .WithOne();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.OwnsOne(x => x.Location, locationBuilder =>
        {
            locationBuilder.ToTable("AdLocations");
            locationBuilder.WithOwner().HasForeignKey("AdId");
        });

        builder.OwnsOne(x => x.Price, p =>
        {
            p.ToTable("AdPrices");
            p.WithOwner().HasForeignKey("AdId");
            p.Property(pr => pr.Amount).HasColumnName("Amount");
            p.OwnsOne(x => x.Currency, cur =>
            {
                cur.Property(c => c.CurrencyCode).HasColumnName("Price_CurrencyCode").HasDefaultValue("USD");
            });
        });

        builder.OwnsOne(x => x.ModerationResult, m =>
        {
            m.Property(x => x.DenyReason)
                .HasConversion<string>()
                .HasColumnName("DenyReason");

            m.Property(x => x.Message)
                .HasMaxLength(ModerationResult.MAX_MESSAGE_LENGTH)
                .HasColumnName("ModeratorMessage");

            m.Property(x => x.ModeratorId)
                .HasColumnName("ModeratorId");
        });

        builder
            .Property<List<Guid>>("_images")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Images");
    }
}