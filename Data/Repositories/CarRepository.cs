using Data.Extensions;
using Data.Filters;
using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories;

public class CarRepository : ICarRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CarRepository> _logger;

    public CarRepository(AppDbContext context, ILogger<CarRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddAsync(Car c)
    {
        _logger.LogInformation($"Adding new car...");
        await _context.Cars.AddAsync(c);
        _logger.LogInformation($"New car with id {c.Id} was added...");
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation($"Searching for car with id {id}...");
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            throw new KeyNotFoundException($"Car with id {id} was not found");
        }
        _logger.LogInformation($"Deleting car with id {id}...");
        _context.Cars.Remove(car);
        _logger.LogInformation($"Car with id {id} was deleted");
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Car c)
    {
        _logger.LogInformation($"Updating car...");
        _context.Cars.Update(c);
        _logger.LogInformation($"Car with id {c.Id} was updated");
        await _context.SaveChangesAsync();
    }

    public async Task<Car> GetByIdAsync(Guid id)
    {
        _logger.LogInformation($"Searching for car with id {id}...");
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            _logger.LogInformation($"Car with id {id} was found");
            return car;
        }
        throw new KeyNotFoundException($"Car with id {id} was not found");
    }

    public async Task<List<Car>> GetAllAsync(CarFilter carFilter)
    {
        _logger.LogInformation($"Getting all cars...");
        return await _context.Cars.Filter(carFilter).AsNoTracking().ToListAsync();
    }

    
}