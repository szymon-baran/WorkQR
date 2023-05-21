namespace WorkQR.Application
{
    public class CompanyEmployeeVM
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public Guid PositionId { get; set; }
        public string RegistrationCode { get; set; } = "";
        public Guid QrAuthorizationKey { get; set; }
        public bool IsDisabled { get; set; }
        public int VacationDaysPerYear { get; set; }
    }
}
