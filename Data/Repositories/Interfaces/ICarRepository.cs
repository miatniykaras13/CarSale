using Data.Filters;
using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ICarRepository
{
    public Task AddAsync(Car c);
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(Car c);
    public Task<Car> GetByIdAsync(Guid id);
    public Task<List<Car>> GetAllAsync(CarFilter carFilter);
}