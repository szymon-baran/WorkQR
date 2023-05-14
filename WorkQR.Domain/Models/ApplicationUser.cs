using Microsoft.AspNetCore.Identity;

namespace WorkQR.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public Guid? PositionId { get; set; }
        public Guid QrAuthorizationKey { get; set; } = Guid.NewGuid();
        public string RegistrationCode { get; set; } = "";
        public DateTime? ContractSignDate { get; set; }
        public int? VacationDaysPerYear { get; set; }
        public virtual Position? Position { get; set; }
        public virtual ICollection<WorktimeEvent> WorktimeEvents { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
    }
}
