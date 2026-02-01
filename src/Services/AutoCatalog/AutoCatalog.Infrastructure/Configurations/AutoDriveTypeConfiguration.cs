using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class AutoDriveTypeConfiguration : IEntityTypeConfiguration<AutoDriveType>
{
    public void Configure(EntityTypeBuilder<AutoDriveType> builder)
    {
        builder.HasKey(t => t.Id);

        builder
            .HasMany(t => t.Cars)
            .WithOne(t => t.DriveType)
            .HasForeignKey(t => t.DriveTypeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(t => t.Name).IsUnique();
    }
}