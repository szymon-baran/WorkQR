using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WorkQR.Application;
using WorkQR.EntityFramework;

namespace WorkQR.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IWorktimeEventService _worktimeEventService;
        private readonly IPositionService _positionService;

        public CompanyController(IApplicationUserService applicationUserService, IWorktimeEventService worktimeEventService, IPositionService positionService)
        {
            _applicationUserService = applicationUserService;
            _worktimeEventService = worktimeEventService;
            _positionService = positionService;
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
    }
}