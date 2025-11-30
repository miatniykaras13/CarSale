using AdService.Domain.Aggregates;
using AdService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdService.Infrastructure.Postgres.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message).IsRequired().HasMaxLength(Comment.MAX_MESSAGE_LENGTH);

        builder
            .HasOne<Ad>()
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.AdId);
    }
}