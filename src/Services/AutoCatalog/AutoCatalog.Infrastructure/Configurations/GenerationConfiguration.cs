using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class GenerationConfiguration : IEntityTypeConfiguration<Generation>
{
    public void Configure(EntityTypeBuilder<Generation> builder)
    {
        builder.HasKey(g => g.Id);

        builder
            .HasOne(g => g.Model)
            .WithMany(e => e.Generations)
            .HasForeignKey(g => g.ModelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(g => g.Engines)
            .WithOne(e => e.Generation)
            .HasForeignKey(e => e.GenerationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}