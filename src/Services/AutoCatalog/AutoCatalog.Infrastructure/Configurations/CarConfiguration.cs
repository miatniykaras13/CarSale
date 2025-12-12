using AutoCatalog.Domain.Transport.Cars;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasOne(c => c.Engine)
            .WithMany(e => e.Cars)
            .HasForeignKey(c => c.EngineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(c => c.Generation)
            .WithMany()
            .HasForeignKey(c => c.GenerationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(c => c.Model)
            .WithMany()
            .HasForeignKey(c => c.ModelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(c => c.Brand)
            .WithMany()
            .HasForeignKey(c => c.BrandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(c => c.AutoDriveType)
            .HasConversion<string>();

        builder
            .Property(c => c.TransmissionType)
            .HasConversion<string>();

        builder
            .Property(x => x.Consumption)
            .HasPrecision(18, 1);

        builder
            .Property(x => x.Acceleration)
            .HasPrecision(18, 1);


        builder.OwnsOne(c => c.Dimensions);
    }
}