using WorkQR.Domain.Dictionaries;

namespace WorkQR.Domain.Models
{
    public class Vacation
    {
        public Guid Id { get; set; }
        public string RequestDescription { get; set; } = "";
        public bool IsApproved { get; set; }
        public string RejectionDescription { get; set; } = "";
        public bool IsRejected { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public VacationType VacationType { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
