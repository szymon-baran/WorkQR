namespace WorkQR.Application
{
    public class EmployeeDTO
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PositionName { get; set; } = "";
        public string RegistrationCode { get; set; } = "";
        public bool IsDisabled { get; set; }
    }
}
