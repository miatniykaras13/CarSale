﻿using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext : DbContext 
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    

}