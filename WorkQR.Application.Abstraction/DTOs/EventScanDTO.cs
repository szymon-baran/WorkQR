using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public class EventScanDTO
    {
        public EventType EventType { get; set; }
        public double BreakMinutesLeftToday { get; set; }
    }
}
