using Data.Filters;
using BusinessLogic.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CarSaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllByAsync([FromQuery]UserFilter userFilter)
        {
            var list = await _userService.GetAllAsync(userFilter);
            return Ok(list);
        }
    }
}
