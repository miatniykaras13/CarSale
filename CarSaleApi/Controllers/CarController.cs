using Data.Models;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using BusinessLogic.Services.Contracts;
using BusinessLogic.DTO;
using Microsoft.AspNetCore.Authorization;

namespace CarSaleApi.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CarController : ControllerBase
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Car>> GetByIdAsync([FromRoute] Guid id)
    {
        var car = await _carService.GetByIdAsync(id);
        return Ok(car);
    }

    [HttpGet]
    public async Task<ActionResult<List<CarDto>>> GetAllAsync()
    {
        var listDto = await _carService.GetAllAsync();
        return Ok(listDto);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] CreateCarDto createCarDto)
    {
        var car = await _carService.AddAsync(createCarDto);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = car.Id }, car);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await _carService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] CarDto c)
    {
        if (id != c.Id)
            return BadRequest("Id doesn't match");
        await _carService.UpdateAsync(c);
        return Ok();
    }



}