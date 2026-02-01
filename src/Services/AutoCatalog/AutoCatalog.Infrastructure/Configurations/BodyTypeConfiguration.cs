using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class BodyTypeConfiguration : IEntityTypeConfiguration<BodyType>
{
    public void Configure(EntityTypeBuilder<BodyType> builder)
    {
        builder.HasKey(t => t.Id);

        builder
            .HasMany(t => t.Cars)
            .WithOne(t => t.BodyType)
            .HasForeignKey(t => t.BodyTypeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(t => t.Name).IsUnique();
    }
}