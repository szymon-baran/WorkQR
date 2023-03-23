using System.ComponentModel.DataAnnotations;

namespace WorkQR.Application
{
    public class CompanyRegisterVM
    {
        [Required]
        public string CompanyName { get; set; } = "";
        [Required]
        public string ModeratorUsername { get; set; } = "";
        [Required]
        public string ModeratorPassword { get; set; } = "";
        [Required]
        public string ModeratorEmail { get; set; } = "";
    }
}
