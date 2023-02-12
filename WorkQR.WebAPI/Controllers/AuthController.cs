using Azure;
using Microsoft.AspNetCore.Mvc;
using WorkQR.Application;

namespace WorkQR.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserLoginVM model)
        {
            var userLogin = await _authService.LoginAsync(model);
            return userLogin != null ? Ok(userLogin) : Unauthorized("Logowanie nieudane.");
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterVM model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            else if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            else
                return Ok(true);
        }
    }
}