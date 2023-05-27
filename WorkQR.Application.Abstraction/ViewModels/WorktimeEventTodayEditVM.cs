using System.ComponentModel.DataAnnotations;

namespace WorkQR.Application
{
    public class WorktimeEventTodayEditVM
    {
        [Required]
        public Guid Id { get; set; }
        public string Description { get; set; } = "";
    }
}
