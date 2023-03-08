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
            try
            {
                var userLogin = await _authService.LoginAsync(model);
                return Ok(userLogin);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterVM model)
        {
            try
            {
                var result = await _authService.RegisterAsync(model);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, result?.Errors.First().Description ?? "Wystąpił błąd");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("refreshAccessToken")]
        public async Task<ActionResult<UserTokenDTO>> RefreshAccessToken(UserTokenVM model)
        {
            try
            {
                var result = await _authService.RefreshAccessTokenAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}