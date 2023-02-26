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
    }
}