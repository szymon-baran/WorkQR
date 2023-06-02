using Microsoft.AspNetCore.Mvc;

namespace WorkQR.Application
{
    public interface IWorktimeEventService
    {
        Task<List<WorktimeEventDTO>> GetUserWorktimeEventsToday();
        Task<List<WorktimeEventDTO>> GetEmployeeWorkHours(GetUserDetailsVM model);
        Task<WorktimeEventsTimestampsDTO> GetUserWorktimeEventsBetweenDatesForCalendar(DaysSpanVM model);
        Task<byte[]> GetCompanyRaportForDate(ModeratorRaportDocumentVM model);
        Task<EmployeePresenceDTO> GetEmployeePresenceData(RaportDocumentVM model);
        Task<EmployeeWorkTimeComparisonDTO> GetEmployeeWorkTimeComparisonData(RaportDocumentVM model);
        Task<List<EmployeePresenceDTO>> GetModeratorEmployeesPresenceData(ModeratorRaportDocumentVM model);
        Task<List<ModeratorEmployeeWorkedHoursDTO>> GetEmployeesWorkedHoursData(ModeratorRaportDocumentVM model);
        Task<List<ModeratorEmployeeWarningDTO>> GetModeratorEmployeeWarnings(RaportDocumentVM model);
        Task UpdateTodayEventDescription(WorktimeEventTodayEditVM model);
    }
}
