using WorkQR.Application;
using WorkQR.Domain.Models;

namespace WorkQR.Infrastructure.Abstraction
{
    public interface IWorktimeEventRepository : IRepository<WorktimeEvent>
    {
        Task<IEnumerable<WorktimeEvent>> GetWorktimeEvents(GetUserDetailsVM model);
    }
}