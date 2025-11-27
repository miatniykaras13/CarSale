using AdService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdService.Infrastructure.Postgres.Data.Configurations;

public class CarOptionConfiguration : IEntityTypeConfiguration<CarOption>
{
    public void Configure(EntityTypeBuilder<CarOption> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(CarOption.MAX_NAME_LENGTH);

        builder.Property(x => x.TechnicalName)
            .IsRequired()
            .HasMaxLength(CarOption.MAX_TECHNAME_LENGTH);

        builder.Property(x => x.OptionType)
            .IsRequired()
            .HasConversion<string>();
    }
}