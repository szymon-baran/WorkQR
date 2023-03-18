using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public class WorktimeEventsTimestampsDTO
    {
        public List<QTimestampDTO> Timestamps { get; set; } = new();
        public double WorkedMinutes { get => Timestamps.Where(x => x.EventType == EventType.StartWork || x.EventType == EventType.EndBreak).Sum(x => x.Duration); }
        public double BreakMinutes { get => Timestamps.Where(x => x.EventType == EventType.StartBreak).Sum(x => x.Duration); }
    }
}
