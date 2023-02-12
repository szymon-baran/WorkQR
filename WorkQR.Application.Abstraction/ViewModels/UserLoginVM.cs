using System.ComponentModel.DataAnnotations;

namespace WorkQR.Application
{
    public class UserLoginVM
    {
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
        [Required]
        public bool RememberMe { get; set; } = false;
    }
}
