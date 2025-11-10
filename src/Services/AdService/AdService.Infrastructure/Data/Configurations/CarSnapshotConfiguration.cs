using AdService.Domain.Ads.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdService.Infrastructure.Data.Configurations;

public class CarSnapshotConfiguration : IEntityTypeConfiguration<CarSnapshot>
{
    public void Configure(EntityTypeBuilder<CarSnapshot> builder)
    {
        builder.HasKey(x => x.AdId);
        
        
    }
}