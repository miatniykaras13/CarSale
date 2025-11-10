using AdService.Domain.Ads.Aggregates;
using AdService.Domain.Ads.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdService.Infrastructure.Data.Configurations;

public class AdConfiguration : IEntityTypeConfiguration<Ad>
{
    public void Configure(EntityTypeBuilder<Ad> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(a => a.Car)
            .WithOne()
            .HasForeignKey<CarSnapshot>(x => x.AdId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}