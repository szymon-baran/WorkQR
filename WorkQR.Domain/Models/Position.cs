namespace WorkQR.Domain
{
    public class Position
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public double TimeBasis { get; set; } = 1.00;
        public int BreakMinsPerDay { get; set; } = 15;
        public Guid? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
