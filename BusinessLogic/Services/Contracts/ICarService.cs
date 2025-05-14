using BusinessLogic.DTO;
using Data.Models;

namespace BusinessLogic.Services.Contracts;

public interface ICarService
{
    public Task<Car> AddAsync(CreateCarDto c, Guid userId);
    public Task UpdateAsync(CarDto c);
    public Task<CarDto> GetByIdAsync(Guid id);
    public Task<List<CarDto>> GetAllAsync();
    public Task DeleteAsync(Guid id);
}