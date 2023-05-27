using Microsoft.AspNetCore.Mvc;

namespace WorkQR.Application
{
    public interface IWorktimeEventService
    {
        Task<List<WorktimeEventDTO>> GetUserWorktimeEventsToday(string userName);
        Task<List<WorktimeEventDTO>> GetEmployeeWorkHours(GetUserDetailsVM model);
        Task<WorktimeEventsTimestampsDTO> GetUserWorktimeEventsBetweenDatesForCalendar(DaysSpanVM model, string userName);
        Task<byte[]> GetCompanyRaportForDate(RaportDocumentVM model, string userName);
        Task<List<ModeratorEmployeePresenceDTO>> GetEmployeesPresenceData(RaportDocumentVM model, string userName);
        Task<List<ModeratorEmployeeWorkedHoursDTO>> GetEmployeesWorkedHoursData(RaportDocumentVM model, string userName);
        Task UpdateTodayEventDescription(string userName, WorktimeEventTodayEditVM model);
    }
}
