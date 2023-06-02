using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkQR.Application;
using WorkQR.Domain.Models;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Presentation.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WorktimeEventController : ControllerBase
    {
        private readonly IWorktimeEventService _worktimeEventService;

        public WorktimeEventController(IWorktimeEventService worktimeEventService)
        {
            _worktimeEventService = worktimeEventService;
        }


        [HttpGet("getUserWorktimeEventsToday")]
        public async Task<ActionResult<List<WorktimeEventDTO>>> GetUserWorktimeEventsToday()
        {
            try
            {
                List<WorktimeEventDTO> worktimeEventsList = await _worktimeEventService.GetUserWorktimeEventsToday();
                return Ok(worktimeEventsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getUserWorktimeEventsBetweenDates")]
        public async Task<ActionResult<WorktimeEventsTimestampsDTO>> GetUserWorktimeEventsBetweenDates([FromQuery]DaysSpanVM model)
        {
            try
            {
                WorktimeEventsTimestampsDTO worktimeEventsDTO = await _worktimeEventService.GetUserWorktimeEventsBetweenDatesForCalendar(model);
                return Ok(worktimeEventsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("updateTodayEventDescription")]
        public async Task<ActionResult<WorktimeEventsTimestampsDTO>> UpdateTodayEventDescription(WorktimeEventTodayEditVM model)
        {
            try
            {
                await _worktimeEventService.UpdateTodayEventDescription(model);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}