using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(c => c.FirstName).HasMaxLength(30);
        builder.Property(c => c.LastName).HasMaxLength(30);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(30);
        builder.HasKey(c => c.Id);

        builder.
            HasMany(u => u.Cars).
            WithOne().
            HasForeignKey(c => c.OwnerId).
            OnDelete(DeleteBehavior.Cascade);
    }
}