using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkQR.Application;
using WorkQR.EntityFramework;

namespace WorkQR.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.QRScanner)]
    public class QRScannerController : ControllerBase
    {
        public QRScannerController()
        {

        }

        [HttpPost("scan")]
        public async Task<ActionResult<UserDTO>> Scan(string qrAuthorizationKey)
        {
            return Ok();
        }
    }
}