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
            try
            {
                var companyEmployees = await _applicationUserService.GetCompanyEmployeesForModerator();
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
            try
            {
                var companyEmployees = await _applicationUserService.GetCompanyEmployeesToSelect();
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
            try
            {
                var companyEmployees = await _applicationUserService.GetCompanyInactiveAccounts();
                return Ok(companyEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #region Positions

        [HttpGet("getCompanyPositions")]
        public async Task<ActionResult<List<CompanyPositionDTO>>> GetCompanyPositions()
        {
            try
            {
                var companyPositions = await _positionService.GetCompanyPositionsByUserName();
                return Ok(companyPositions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getCompanyPositionsForUserToSelect")]
        public async Task<ActionResult<List<SelectDTO<Guid>>>> GetCompanyPositionsForUserToSelect()
        {
            try
            {
                var companyPositions = await _positionService.GetCompanyPositionsForUserToSelect();
                return Ok(companyPositions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion Positions

        [HttpPost("updateCompanyEmployees")]
        public async Task<ActionResult> UpdateCompanyEmployees(List<CompanyEmployeeVM> model)
        {
            try
            {
                await _applicationUserService.UpdateCompanyEmployees(model);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getEmployeeWorktimeEvents")]
        public async Task<ActionResult<List<FullEmployeeDTO>>> GetEmployeeWorkHours([FromQuery]GetUserDetailsVM model)
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
        public async Task<IActionResult> GetCompanyRaportForDate([FromQuery]ModeratorRaportDocumentVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                var bytes = await _worktimeEventService.GetCompanyRaportForDate(model);
                return File(bytes, "application/pdf", $"{DateTime.Today.ToShortDateString()}-raport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getEmployeesPresenceData")]
        public async Task<ActionResult<EmployeePresenceDTO>> GetEmployeesPresenceData([FromQuery]ModeratorRaportDocumentVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                var presenceData = await _worktimeEventService.GetModeratorEmployeesPresenceData(model);
                return Ok(presenceData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getEmployeesWorkedHoursData")]
        public async Task<ActionResult<ModeratorEmployeeWorkedHoursDTO>> GetEmployeesWorkedHoursData([FromQuery]ModeratorRaportDocumentVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                var workedHours = await _worktimeEventService.GetEmployeesWorkedHoursData(model);
                return Ok(workedHours);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addEmployee")]
        public async Task<ActionResult<Guid>> AddEmployee(EmployeeAddVM model)
        {
            try
            {
                await _authService.AddEmployee(model);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #region Vacation

        [HttpGet("getVacationRequests")]
        public async Task<ActionResult<List<ModeratorVacationRequestDTO>>> GetVacationRequests()
        {
            try
            {
                var vacationRequests = await _vacationService.GetModeratorAllVacationRequests();
                return Ok(vacationRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getCompanyEmployeeVacationRequests")]
        public async Task<ActionResult<List<ModeratorVacationRequestDTO>>> GetCompanyEmployeeVacationRequests([FromQuery]GetUserDetailsVM model)
        {
            try
            {
                var vacationRequests = await _vacationService.GetModeratorVacationRequestsByUser(model);
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
            try
            {
                await _vacationService.AcceptVacationRequest(id);
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
            try
            {
                await _vacationService.RejectVacationRequest(model);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion Vacation

        #region Warnings

        [HttpGet("getEmployeesWarnings")]
        public async Task<ActionResult<List<ModeratorEmployeeWarningDTO>>> GetEmployeesWarnings([FromQuery] RaportDocumentVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                var vacationRequests = await _worktimeEventService.GetModeratorEmployeeWarnings(model);
                return Ok(vacationRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

    }
}