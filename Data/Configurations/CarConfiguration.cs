using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.Property(c => c.Brand).HasMaxLength(20);
        builder.HasKey(c => c.Id);

        builder.
            HasOne(c => c.Owner).
            WithMany(u => u.Cars);

    }
}