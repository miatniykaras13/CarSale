using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .HasMany(b => b.Models)
            .WithOne(m => m.Brand)
            .HasForeignKey(m => m.BrandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.Name).IsUnique();
    }
}