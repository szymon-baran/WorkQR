using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkQR.Application;
using WorkQR.Domain.Models;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Presentation.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IQRService _qrService;

        public UserController(IQRService qrScannerService)
        {
            _qrService = qrScannerService;
        }


        [HttpGet("getQRAuthorizationKey")]
        public async Task<ActionResult<Guid>> GetQRAuthorizationKey()
        {
            try
            {
                string userName = User.Identity.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                Guid qrAuthorizationKey = await _qrService.GetQRAuthorizationKeyByUserName(userName);
                return Ok(qrAuthorizationKey);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}