using BusinessLogic.Services.Contracts;
using CarSaleApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CarSaleApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        []
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmailLoginRequest request)
        {
            var token = await _userService.Login(request.Email, request.Password);

            HttpContext.Response.Cookies.Append("good-cookies", token);
            return Ok();
        }

       


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _userService.Register(request.FirstName, request.LastName, request.Email, request.Password);
            return Ok();
        }


    }
}
