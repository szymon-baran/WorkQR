using Microsoft.AspNetCore.Mvc;

namespace WorkQR.Application
{
    public interface IWorktimeEventService
    {
        Task<List<WorktimeEventDTO>> GetUserWorktimeEventsToday(string userName);
        Task<List<WorktimeEventDTO>> GetEmployeeWorkHours(GetEventsVM model);
        Task<WorktimeEventsTimestampsDTO> GetUserWorktimeEventsBetweenDatesForCalendar(DaysSpanVM model, string userName);
        Task<byte[]> GetCompanyRaportForDate(RaportDocumentVM model, string userName);
    }
}
