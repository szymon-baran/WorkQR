namespace WorkQR.Domain
{
    public class Vacation
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = "";
        public bool IsApproved { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
