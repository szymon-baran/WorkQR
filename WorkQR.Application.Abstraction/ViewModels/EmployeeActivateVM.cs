using System.ComponentModel.DataAnnotations;

namespace WorkQR.Application
{
    public class EmployeeActivateVM
    {
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";
        [Required]
        public string RegistrationCode { get; set; } = "";
    }
}
