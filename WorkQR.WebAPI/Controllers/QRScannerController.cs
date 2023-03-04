using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkQR.Application;
using WorkQR.Domain;
using WorkQR.EntityFramework;

namespace WorkQR.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.QRScanner)]
    public class QRScannerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQRScannerService _qrScannerService;

        public QRScannerController(UserManager<ApplicationUser> userManager, IQRScannerService qrScannerService)
        {
            _userManager = userManager;
            _qrScannerService = qrScannerService;
        }

        [HttpPost("scan")]
        public async Task<ActionResult<EventScanDTO>> Scan(string qrAuthorizationKey)
        {
            try
            {
                EventScanDTO addedEvent = await _qrScannerService.Scan(Guid.Parse(qrAuthorizationKey));
                return Ok(addedEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}