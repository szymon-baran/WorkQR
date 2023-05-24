using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using WorkQR.Application;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Presentation.WebAPI.Controllers
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
        private readonly IVacationService _vacationService;

        public CompanyModerationController(IApplicationUserService applicationUserService,
                                           IWorktimeEventService worktimeEventService,
                                           IQRService qrService,
                                           IPositionService positionService,
                                           IAuthService authService,
                                           IVacationService vacationService)
        {
            _applicationUserService = applicationUserService;
            _worktimeEventService = worktimeEventService;
            _qrService = qrService;
            _positionService = positionService;
            _authService = authService;
            _vacationService = vacationService;
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

        [HttpGet("getCompanyEmployeesToSelect")]
        public async Task<ActionResult<List<SelectDTO<string>>>> GetCompanyEmployeesToSelect()
        {
            string userName = User.Identity.Name;
            try
            {
                var companyEmployees = await _applicationUserService.GetCompanyEmployeesToSelect(userName);
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

        [HttpGet("getEmployeesPresenceData")]
        public async Task<ActionResult<EmployeePresenceDTO>> GetEmployeesPresenceData([FromQuery]RaportDocumentVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                var presenceData = await _worktimeEventService.GetEmployeesPresenceData(model, userName);
                return Ok(presenceData);
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

        #region Vacation

        [HttpGet("getVacationRequests")]
        public async Task<ActionResult<List<VacationRequestModeratorDTO>>> GetVacationRequests()
        {
            string userName = User.Identity.Name;
            try
            {
                var vacationRequests = await _vacationService.GetModeratorVacationRequestsByUsername(userName);
                return Ok(vacationRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("acceptVacationRequest")]
        public async Task<ActionResult> AcceptVacationRequest(Guid id)
        {
            string userName = User.Identity.Name;
            try
            {
                await _vacationService.AcceptVacationRequest(userName, id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("rejectVacationRequest")]
        public async Task<ActionResult> RejectVacationRequest(ModeratorVacationRejectionVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                await _vacationService.RejectVacationRequest(userName, model);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion Vacation

    }
}