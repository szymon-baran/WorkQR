using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public class EventScanDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = "";
        public EventType EventType { get; set; }
        public double BreakMinutesLeftToday { get; set; }
    }
}
