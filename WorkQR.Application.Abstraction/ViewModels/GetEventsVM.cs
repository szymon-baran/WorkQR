using System.ComponentModel.DataAnnotations;

namespace WorkQR.Application
{
    public class GetUserDetailsVM
    {
        [Required]
        public string UserId { get; set; } = "";
        public DateTime DateFrom { get; set; } = DateTime.Now.AddDays(-7);
        public DateTime DateTo { get; set; } = DateTime.Now;
        public string? Description { get; set; } = "";
    }
}
