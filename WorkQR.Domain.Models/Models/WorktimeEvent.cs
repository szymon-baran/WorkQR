using WorkQR.Domain.Dictionaries;

namespace WorkQR.Domain.Models
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
