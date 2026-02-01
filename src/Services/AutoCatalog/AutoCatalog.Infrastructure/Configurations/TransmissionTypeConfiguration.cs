using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoCatalog.Infrastructure.Configurations;

public class TransmissionTypeConfiguration : IEntityTypeConfiguration<TransmissionType>
{
    public void Configure(EntityTypeBuilder<TransmissionType> builder)
    {
        builder.HasKey(t => t.Id);

        builder
            .HasMany(t => t.Cars)
            .WithOne(t => t.TransmissionType)
            .HasForeignKey(t => t.TransmissionTypeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(t => t.Name).IsUnique();
    }
}