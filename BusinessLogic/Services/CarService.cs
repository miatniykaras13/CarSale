using Data.Models;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using BusinessLogic.Services.Contracts;
using AutoMapper;
using BusinessLogic.DTO;
using Data.Filters;

namespace BusinessLogic.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _repository;
    private readonly IUserService _userService;

    private readonly ILogger<CarService> _logger;
    private readonly IMapper _mapper;


    public CarService(ICarRepository repository, ILogger<CarService> logger, IMapper mapper, IUserService userService)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
    }


    public async Task<Car> AddAsync(CreateCarDto createCarDto, Guid userId)
    {
        var car = _mapper.Map<Car>(createCarDto);
        car.UserId = userId;

        await _repository.AddAsync(car);
        return car;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<CarDto>> GetAllAsync(CarFilter carFilter)
    {
        var list = await _repository.GetAllAsync(carFilter);
        var listDto = _mapper.Map<List<CarDto>>(list);
        return listDto;
    }

    public async Task<CarDto> GetByIdAsync(Guid id)
    {
        var carDto = _mapper.Map<CarDto>(await _repository.GetByIdAsync(id));
        return carDto;
    }
    public async Task UpdateAsync(CarDto c)
    {
        var car = _mapper.Map<Car>(c);
        await _repository.UpdateAsync(car);
    }
}