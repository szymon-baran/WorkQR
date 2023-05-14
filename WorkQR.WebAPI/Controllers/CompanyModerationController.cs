using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using WorkQR.Application;
using WorkQR.EntityFramework;

namespace WorkQR.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.Moderator)]
    public class CompanyModerationController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IWorktimeEventService _worktimeEventService;
        private readonly IQRService _qrService;
        private readonly IPositionService _positionService;
        private readonly IAuthService _authService;

        public CompanyModerationController(IApplicationUserService applicationUserService,
                                           IWorktimeEventService worktimeEventService,
                                           IQRService qrService,
                                           IPositionService positionService,
                                           IAuthService authService)
        {
            _applicationUserService = applicationUserService;
            _worktimeEventService = worktimeEventService;
            _qrService = qrService;
            _positionService = positionService;
            _authService = authService;
        }

        [HttpGet("getCompanyEmployees")]
        public async Task<ActionResult<List<FullEmployeeDTO>>> GetCompanyEmployees()
        {
            string userName = User.Identity.Name;
            try
            {
                var companyEmployees = await _applicationUserService.GetCompanyEmployeesForModerator(userName);
                return Ok(companyEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getCompanyInactiveAccounts")]
        public async Task<ActionResult<List<FullEmployeeDTO>>> GetCompanyInactiveAccounts()
        {
            string userName = User.Identity.Name;
            try
            {
                var companyEmployees = await _applicationUserService.GetCompanyInactiveAccounts(userName);
                return Ok(companyEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getCompanyPositionsForUserToSelect")]
        public async Task<ActionResult<List<SelectDTO<Guid>>>> GetCompanyPositionsForUserToSelect()
        {
            string userName = User.Identity.Name;
            try
            {
                var companyPositions = await _positionService.GetCompanyPositionsForUserToSelect(userName);
                return Ok(companyPositions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("updateCompanyEmployees")]
        public async Task<ActionResult> UpdateCompanyEmployees(List<CompanyEmployeeVM> model)
        {
            string userName = User.Identity.Name;
            try
            {
                await _applicationUserService.UpdateCompanyEmployees(userName, model);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getEmployeeWorktimeEvents")]
        public async Task<ActionResult<List<FullEmployeeDTO>>> GetEmployeeWorkHours([FromQuery]GetEventsVM model)
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

        [HttpGet("getCompanyRaportForDate")]
        public async Task<IActionResult> GetCompanyRaportForDate([FromQuery]RaportDocumentVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                var bytes = await _worktimeEventService.GetCompanyRaportForDate(model, userName);
                return File(bytes, "application/pdf", $"{DateTime.Today.ToShortDateString()}-raport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addEmployee")]
        public async Task<ActionResult<Guid>> AddEmployee(EmployeeAddVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                await _authService.AddEmployee(model, userName);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}