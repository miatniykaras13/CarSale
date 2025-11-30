using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FileInfo = FileManagement.Grpc.Models.FileInfo;

namespace FileManagement.Grpc.Data;

public class FileInfoConfiguration : IEntityTypeConfiguration<FileInfo>
{
    public void Configure(EntityTypeBuilder<FileInfo> builder)
    {
        builder
            .HasMany(x => x.Children)
            .WithOne(x => x.Parent)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}