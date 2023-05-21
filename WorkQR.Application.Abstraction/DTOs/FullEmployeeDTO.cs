namespace WorkQR.Application
{
    public class FullEmployeeDTO
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public Guid PositionId { get; set; }
        public string RegistrationCode { get; set; } = "";
        public Guid QrAuthorizationKey { get; set; }
        public DateTime? LastActivity { get; set; }
        public bool IsOnVacation { get; set; }
        public int VacationDaysPerYear { get; set; }
        public int VacationDaysThisYearLeft { get; set; }
    }
}
