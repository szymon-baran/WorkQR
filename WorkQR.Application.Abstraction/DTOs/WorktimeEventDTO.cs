using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public class WorktimeEventDTO
    {
        public Guid Id { get; set; }
        public EventType EventType { get; set; }
        public string EventTypeName { get; set; }
        public DateTime EventTime { get; set; }
        public string Description { get; set; } = "";
        public double DurationInSecs { get; set; } = 0;
    }
}
