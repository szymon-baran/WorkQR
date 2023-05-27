using WorkQR.Domain.Dictionaries;

namespace WorkQR.Application
{
    public class EventScanDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = "";
        public EventType EventType { get; set; }
        public double BreakMinutesLeftToday { get; set; }
        public bool IsOnVacation { get; set; }
        public DateTime? VacationTo { get; set; }
    }
}
