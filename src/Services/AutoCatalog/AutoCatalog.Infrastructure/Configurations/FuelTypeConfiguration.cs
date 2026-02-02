using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class FuelTypeConfiguration : IEntityTypeConfiguration<FuelType>
{
    public void Configure(EntityTypeBuilder<FuelType> builder)
    {
        builder.HasKey(t => t.Id);

        builder
            .HasMany(t => t.Engines)
            .WithOne(t => t.FuelType)
            .HasForeignKey(t => t.FuelTypeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(t => t.Name).IsUnique();
    }
}