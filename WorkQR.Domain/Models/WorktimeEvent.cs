using WorkQR.Dictionaries;

namespace WorkQR.Domain
{
    public class WorktimeEvent
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public EventType EventType { get; set; }
        public DateTime EventTime { get; set; }
        public string Description { get; set; } = "";
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
