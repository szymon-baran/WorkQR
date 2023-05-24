namespace WorkQR.Domain.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public virtual ICollection<Position> Positions { get; set; }
    }
}
