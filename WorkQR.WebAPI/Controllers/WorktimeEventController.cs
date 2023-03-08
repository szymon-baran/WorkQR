using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkQR.Application;
using WorkQR.Domain;
using WorkQR.EntityFramework;

namespace WorkQR.WebAPI.Controllers
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


        [HttpGet("getUserWorktimeEventsBetweenDates")]
        public async Task<ActionResult<QTimestampDTO>> GetUserWorktimeEventsBetweenDates([FromQuery]DaysSpanVM model)
        {
            try
            {
                string userName = User.Identity.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                List<QTimestampDTO> timestamps = await _worktimeEventService.GetUserWorktimeEventsBetweenDates(model, userName);
                return Ok(timestamps);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}