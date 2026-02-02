using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class EngineConfiguration : IEntityTypeConfiguration<Engine>
{
    public void Configure(EntityTypeBuilder<Engine> builder)
    {
        builder.HasKey(e => e.Id);

        builder
            .HasOne(e => e.Generation)
            .WithMany(e => e.Engines)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(e => e.Cars)
            .WithOne(c => c.Engine)
            .HasForeignKey(c => c.EngineId);
        
        builder.HasIndex(t => t.Name).IsUnique();
    }
}