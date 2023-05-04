using System.ComponentModel.DataAnnotations;

namespace WorkQR.Application
{
    public class EmployeeAddVM
    {
        [Required]
        public string Username { get; set; } = "";
        [EmailAddress]
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public Guid PositionId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
