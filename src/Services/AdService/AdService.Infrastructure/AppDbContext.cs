using AdService.Domain.Ads.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace AdService.Infrastructure;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Ad> Ads { get; set; }
    
    
    
}