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

        public CompanyEmployeeController(IApplicationUserService applicationUserService, IVacationService vacationService)
        {
            _applicationUserService = applicationUserService;
            _vacationService = vacationService;
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
    }
}