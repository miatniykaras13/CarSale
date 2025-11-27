using FileManagement.Grpc.Models;
using Microsoft.EntityFrameworkCore;
using FileInfo = FileManagement.Grpc.Models.FileInfo;

namespace FileManagement.Grpc.Data;

public class FileManagementDbContext(DbContextOptions<FileManagementDbContext> options) : DbContext(options)
{
    public DbSet<FileInfo> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}