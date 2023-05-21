using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public class VacationRequestModeratorDTO
    {
        public Guid Id { get; set; }
        public string RequestDescription { get; set; } = "";
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public VacationType VacationType { get; set; }
        public string ApplicationUserId { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Username { get; set; } = "";
    }
}
