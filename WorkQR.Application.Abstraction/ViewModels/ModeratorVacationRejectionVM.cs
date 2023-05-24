using WorkQR.Domain.Dictionaries;

namespace WorkQR.Application
{
    public class ModeratorVacationRejectionVM
    {
        public Guid Id { get; set; }
        public string RejectionDescription { get; set; } = "";
    }
}
