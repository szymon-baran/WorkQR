﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WorkQR.Application;
using WorkQR.Domain.Dictionaries;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Presentation.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CompanyEmployeeController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IVacationService _vacationService;
        private readonly IWorktimeEventService _worktimeEventService;

        public CompanyEmployeeController(IApplicationUserService applicationUserService, IVacationService vacationService, IWorktimeEventService worktimeEventService)
        {
            _applicationUserService = applicationUserService;
            _vacationService = vacationService;
            _worktimeEventService = worktimeEventService;
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

        #region Vacation

        [HttpGet("getVacationRequests")]
        public async Task<ActionResult<List<VacationRequestDTO>>> GetVacationRequests()
        {
            string userName = User.Identity.Name;
            try
            {
                var vacationRequests = await _vacationService.GetVacationRequestsByUsername(userName);
                return Ok(vacationRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getVacationTypes")]
        public ActionResult<List<SelectDTO<VacationType>>> GetVacationTypes()
        {
            try
            {
                var vacationTypes = _vacationService.GetVacationTypes();
                return Ok(vacationTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addVacationRequest")]
        public async Task<ActionResult> AddVacationRequest(VacationRequestVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                await _vacationService.AddVacationRequest(userName, model);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("validateVacationRequest")]
        public async Task<ActionResult<bool>> ValidateVacationRequest([FromQuery] VacationValidationVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                bool result = await _vacationService.ValidateVacationRequest(userName, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion Vacation

        #region Raport

        [HttpGet("getEmployeePresenceData")]
        public async Task<ActionResult<EmployeePresenceDTO>> GetEmployeePresenceData([FromQuery] RaportDocumentVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                var presenceData = await _worktimeEventService.GetEmployeePresenceData(model, userName);
                return Ok(presenceData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getEmployeeWorkTimeComparisonData")]
        public async Task<ActionResult<EmployeeWorkTimeComparisonDTO>> GetEmployeeWorkTimeComparisonData([FromQuery] RaportDocumentVM model)
        {
            string userName = User.Identity.Name;
            try
            {
                var presenceData = await _worktimeEventService.GetEmployeeWorkTimeComparisonData(model, userName);
                return Ok(presenceData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}