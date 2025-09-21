using CarSale.Domain.Ads.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace CarSale.Infrastructure.MsSql;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ad> Ads { get; set; }
}