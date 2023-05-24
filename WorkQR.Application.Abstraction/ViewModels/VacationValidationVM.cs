using WorkQR.Domain.Dictionaries;

namespace WorkQR.Application
{
    public class VacationValidationVM
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public VacationType VacationType { get; set; }
    }
}
