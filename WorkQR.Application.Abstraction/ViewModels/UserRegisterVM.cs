using System.ComponentModel.DataAnnotations;

namespace WorkQR.Application
{
    public class UserRegisterVM
    {
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
        [EmailAddress]
        [Required]
        public string Email { get; set; } = "";
    }
}
