namespace WorkQR.Domain
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
