using AdService.Domain.Aggregates;
using AdService.Domain.Entities;
using AdService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdService.Infrastructure.Data.Configurations;

public class AdConfiguration : IEntityTypeConfiguration<Ad>
{
    public void Configure(EntityTypeBuilder<Ad> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Car, carBuilder =>
        {
            carBuilder.ToTable("CarSnapshots");

            carBuilder.Property(x => x.Brand)
                .HasMaxLength(CarSnapshot.MAX_BRAND_LENGTH)
                .IsRequired();

            carBuilder.Property(x => x.Model)
                .HasMaxLength(CarSnapshot.MAX_MODEL_LENGTH)
                .IsRequired();

            carBuilder.Property(x => x.Generation)
                .HasMaxLength(CarSnapshot.MAX_GENERATION_LENGTH)
                .IsRequired();

            carBuilder.Property(x => x.DriveType).HasConversion<string>();
            carBuilder.Property(x => x.TransmissionType).HasConversion<string>();
            carBuilder.Property(x => x.FuelType).HasConversion<string>();
        });

        builder
            .HasMany(a => a.CarOptions)
            .WithMany();

        builder
            .HasMany(a => a.Comments)
            .WithOne()
            .HasForeignKey(c => c.AdId);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.OwnsOne(x => x.Location);

        builder.OwnsOne(x => x.Price, p =>
        {
            p.OwnsOne(x => x.Currency, cur =>
            {
                cur.Property(c => c.CurrencyCode).HasColumnName("Price_CurrencyCode");
            });
        });

        builder.OwnsOne(x => x.Seller, s =>
        {
            s.Property(x => x.DisplayName)
                .HasMaxLength(SellerSnapshot.MAX_NAME_LENGTH)
                .HasColumnName("Seller_Name");
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