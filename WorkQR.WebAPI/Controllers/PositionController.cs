using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WorkQR.Application;
using WorkQR.EntityFramework;

namespace WorkQR.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.Moderator)]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet("getCompanyPositionsForUser")]
        public async Task<ActionResult<List<FullEmployeeDTO>>> GetCompanyPositionsForUser()
        {
            string userName = User.Identity.Name;
            try
            {
                var companyPositions = await _positionService.GetCompanyPositionsByUserName(userName);
                return Ok(companyPositions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}