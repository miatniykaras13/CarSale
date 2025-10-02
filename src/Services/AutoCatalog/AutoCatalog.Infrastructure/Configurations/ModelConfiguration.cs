using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.HasKey(g => g.Id);

        builder
            .HasOne(m => m.Brand)
            .WithMany(b => b.Models)
            .HasForeignKey(m => m.BrandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(m => m.Generations)
            .WithOne(g => g.Model)
            .HasForeignKey(g => g.ModelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}