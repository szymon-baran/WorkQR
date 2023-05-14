using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public class VacationRequestVM
    {
        public string Description { get; set; } = "";
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public VacationType VacationType { get; set; }
    }
}
