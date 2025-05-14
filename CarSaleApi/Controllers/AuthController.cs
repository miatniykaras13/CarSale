using BusinessLogic.Services.Contracts;
using CarSaleApi.Requests;
using Microsoft.AspNetCore.Authorization;
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



        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("good-cookies");
            return Ok();
        }


        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<ActionResult> DeleteAccount()
        {   
            await _userService.DeleteAsync(Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value));
            Logout();
            return Ok();
        }


    }
}
