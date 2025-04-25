using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarSaleApi.Controllers;


[ApiController]
[Route("[controller]")]
public class CarController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("Hello World!");
    }
}