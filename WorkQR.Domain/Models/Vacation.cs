using WorkQR.Dictionaries;

namespace WorkQR.Domain
{
    public class Vacation
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = "";
        public bool IsApproved { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public VacationType VacationType { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
