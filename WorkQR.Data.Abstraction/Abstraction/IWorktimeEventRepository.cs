using WorkQR.Application;
using WorkQR.Domain;

namespace WorkQR.Data.Abstraction
{
    public interface IWorktimeEventRepository : IRepository<WorktimeEvent>
    {
        Task<IEnumerable<WorktimeEvent>> GetWorktimeEvents(GetEventsVM model);
    }
}