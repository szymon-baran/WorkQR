namespace WorkQR.Application
{
    public interface IWorktimeEventService
    {
        Task<WorktimeEventDTO> GetUserWorktimeEventsBetweenDates(DaysSpanVM model, string userName);
    }
}
