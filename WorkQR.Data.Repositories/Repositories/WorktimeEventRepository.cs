using WorkQR.Application;
using WorkQR.Data.Abstraction;
using WorkQR.Domain;
using WorkQR.EntityFramework;

namespace WorkQR.Data.Repositories
{
    public class WorktimeEventRepository : Repository<WorktimeEvent>, IWorktimeEventRepository
    {
        public WorktimeEventRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WorktimeEvent>> GetWorktimeEvents(GetEventsVM model)
        {
            return await GetWithConditionAsync(x => x.ApplicationUserId == model.UserId
                                                                        && x.EventTime >= model.DateFrom
                                                                        && x.EventTime < model.DateTo
                                                                        && (string.IsNullOrEmpty(model.Description) 
                                                                            || x.Description.Contains(model.Description)));
        }
    }
}