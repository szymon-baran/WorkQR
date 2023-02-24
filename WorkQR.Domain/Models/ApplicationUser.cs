using Microsoft.AspNetCore.Identity;

namespace WorkQR.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid QrAuthorizationKey { get; set; } = Guid.NewGuid();
        public virtual Company? Company { get; set; }
        public virtual ICollection<WorktimeEvent> WorktimeEvents { get; set; }
    }
}
