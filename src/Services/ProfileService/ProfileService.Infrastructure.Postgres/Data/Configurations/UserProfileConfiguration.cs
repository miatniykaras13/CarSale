using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfileService.Domain.Aggregates;

namespace ProfileService.Infrastructure.Postgres.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.KeycloakId)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => x.KeycloakId)
            .IsUnique();

        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => x.Username)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(UserProfile.MAX_NAME_LENGTH);

        builder.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(UserProfile.MAX_SURNAME_LENGTH);

        builder.Property(x => x.Picture)
            .HasMaxLength(500);

        builder.OwnsMany(x => x.Ads, adBuilder =>
        {
            adBuilder.ToTable("AdSnapshots");

            adBuilder.Property(x => x.AdId)
                .IsRequired()
                .HasMaxLength(50);

            adBuilder.Property<Guid>("UserProfileId")
                .HasColumnName("UserProfileId")
                .ValueGeneratedNever();

            adBuilder.HasKey("UserProfileId", "AdId");

            adBuilder.Property(x => x.Title)
                .HasMaxLength(200);

            adBuilder.OwnsOne(x => x.Car, carBuilder =>
            {
                carBuilder.Property(x => x.Brand)
                    .HasColumnName("Car_Brand")
                    .HasMaxLength(100);

                carBuilder.Property(x => x.Model)
                    .HasColumnName("Car_Model")
                    .HasMaxLength(100);

                carBuilder.Property(x => x.Generation)
                    .HasColumnName("Car_Generation")
                    .HasMaxLength(100);

                carBuilder.Property(x => x.Year)
                    .HasColumnName("Car_Year");

                carBuilder.Property(x => x.DriveType)
                    .HasColumnName("Car_DriveType")
                    .HasMaxLength(50);

                carBuilder.Property(x => x.TransmissionType)
                    .HasColumnName("Car_TransmissionType")
                    .HasMaxLength(50);

                carBuilder.Property(x => x.EngineVolume)
                    .HasColumnName("Car_EngineVolume")
                    .HasPrecision(18, 1);

                carBuilder.Property(x => x.FuelType)
                    .HasColumnName("Car_FuelType")
                    .HasMaxLength(50);

                carBuilder.Property(x => x.BodyType)
                    .HasColumnName("Car_BodyType")
                    .HasMaxLength(50);
            });

            adBuilder.OwnsOne(x => x.Price, costBuilder =>
            {
                costBuilder.Property(x => x.Amount)
                    .HasColumnName("Price_Amount");

                costBuilder.OwnsOne(x => x.Currency, currencyBuilder =>
                {
                    currencyBuilder.Property(x => x.Code)
                        .HasColumnName("Price_CurrencyCode")
                        .HasMaxLength(3);
                });
            });
        });

        builder.OwnsOne(x => x.Email, emailBuilder =>
        {
            emailBuilder.Property(x => x.Value)
                .HasColumnName("Email_Value");
        });

        builder.OwnsOne(x => x.PhoneNumber, phoneBuilder =>
        {
            phoneBuilder.Property(x => x.E164)
                .HasColumnName("PhoneNumber_E164");
        });

        builder.Navigation(x => x.Ads)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}