using WorkQR.Application;
using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Models;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Infrastructure.Repositories
{
    public class WorktimeEventRepository : Repository<WorktimeEvent>, IWorktimeEventRepository
    {
        public WorktimeEventRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WorktimeEvent>> GetWorktimeEvents(GetUserDetailsVM model)
        {
            return await GetWithConditionAsync(x => x.ApplicationUserId == model.UserId
                                                                        && x.EventTime.Date >= model.DateFrom.Date
                                                                        && x.EventTime.Date <= model.DateTo.Date
                                                                        && (string.IsNullOrEmpty(model.Description) 
                                                                            || x.Description.Contains(model.Description)));
        }
    }
}