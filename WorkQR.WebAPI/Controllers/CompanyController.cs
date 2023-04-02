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
    public class CompanyController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IWorktimeEventService _worktimeEventService;
        private readonly IQRService _qrService;

        public CompanyController(IApplicationUserService applicationUserService, IWorktimeEventService worktimeEventService, IQRService qrService)
        {
            _applicationUserService = applicationUserService;
            _worktimeEventService = worktimeEventService;
            _qrService = qrService;
        }

        [HttpGet("getCompanyEmployees")]
        public async Task<ActionResult<List<EmployeeDTO>>> GetCompanyEmployees()
        {
            string userName = User.Identity.Name;
            try
            {
                var companyEmployees = await _applicationUserService.GetCompanyEmployees(userName);
                return Ok(companyEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getEmployeeWorktimeEvents")]
        public async Task<ActionResult<List<EmployeeDTO>>> GetEmployeeWorkHours([FromQuery]GetEventsVM model)
        {
            try
            {
                var companyEmployees = await _worktimeEventService.GetEmployeeWorkHours(model);
                return Ok(companyEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("resetUserQRAuthorizationKey")]
        public async Task<ActionResult<Guid>> ResetUserQRAuthorizationKey(string userId)
        {
            try
            {
                var companyEmployees = await _qrService.ResetUserQRAuthorizationKey(userId);
                return Ok(companyEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}