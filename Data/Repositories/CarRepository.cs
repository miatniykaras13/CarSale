using Data.Models;
using Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CarRepository : IRepository<Car>
{
    private readonly AppDbContext _context;

    public CarRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Car c)
    {
        await _context.Cars.AddAsync(c);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(Car c)
    {
        _context.Cars.Remove(c);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Car o)
    {
        _context.Cars.Update(o);
        await _context.SaveChangesAsync();
    }

    public async Task<Car> GetByIdAsync(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if(car != null)
            return car;
        
        throw new Exception("Car not found");
    }

    public async Task<List<Car>> GetAllAsync()
    {
        return await _context.Cars.ToListAsync();
    }
}