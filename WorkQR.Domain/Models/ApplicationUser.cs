using Microsoft.AspNetCore.Identity;

namespace WorkQR.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public Guid? CompanyId { get; set; }
        public Guid QrAuthorizationKey { get; set; } = Guid.NewGuid();
        public virtual Company? Company { get; set; }
    }
}
