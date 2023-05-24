using WorkQR.Domain.Dictionaries;

namespace WorkQR.Application
{
    public class VacationRequestDTO
    {
        public Guid Id { get; set; }
        public string RequestDescription { get; set; } = "";
        public bool IsApproved { get; set; }
        public string RejectionDescription { get; set; } = "";
        public bool IsRejected { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public VacationType VacationType { get; set; }
    }
}
