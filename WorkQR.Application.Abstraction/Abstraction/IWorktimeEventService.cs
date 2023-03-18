namespace WorkQR.Application
{
    public interface IWorktimeEventService
    {
        Task<List<WorktimeEventDTO>> GetUserWorktimeEventsToday(string userName);
        Task<WorktimeEventsTimestampsDTO> GetUserWorktimeEventsBetweenDates(DaysSpanVM model, string userName);
    }
}
