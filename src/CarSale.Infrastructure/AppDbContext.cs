using Microsoft.EntityFrameworkCore;

namespace CarSale.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options);