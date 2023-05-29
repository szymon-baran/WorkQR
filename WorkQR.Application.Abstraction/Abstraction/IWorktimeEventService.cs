using Microsoft.AspNetCore.Mvc;

namespace WorkQR.Application
{
    public interface IWorktimeEventService
    {
        Task<List<WorktimeEventDTO>> GetUserWorktimeEventsToday(string userName);
        Task<List<WorktimeEventDTO>> GetEmployeeWorkHours(GetUserDetailsVM model);
        Task<WorktimeEventsTimestampsDTO> GetUserWorktimeEventsBetweenDatesForCalendar(DaysSpanVM model, string userName);
        Task<byte[]> GetCompanyRaportForDate(ModeratorRaportDocumentVM model, string userName);
        Task<EmployeePresenceDTO> GetEmployeePresenceData(RaportDocumentVM model, string userName);
        Task<EmployeeWorkTimeComparisonDTO> GetEmployeeWorkTimeComparisonData(RaportDocumentVM model, string userName);
        Task<List<EmployeePresenceDTO>> GetModeratorEmployeesPresenceData(ModeratorRaportDocumentVM model, string userName);
        Task<List<ModeratorEmployeeWorkedHoursDTO>> GetEmployeesWorkedHoursData(ModeratorRaportDocumentVM model, string userName);
        Task UpdateTodayEventDescription(string userName, WorktimeEventTodayEditVM model);
    }
}
