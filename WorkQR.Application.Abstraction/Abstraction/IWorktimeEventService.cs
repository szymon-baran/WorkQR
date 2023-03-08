namespace WorkQR.Application
{
    public interface IWorktimeEventService
    {
        Task<List<QTimestampDTO>> GetUserWorktimeEventsBetweenDates(DaysSpanVM model, string userName);
    }
}
