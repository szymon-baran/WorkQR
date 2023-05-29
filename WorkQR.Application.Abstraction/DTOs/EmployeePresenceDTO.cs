namespace WorkQR.Application
{
    public class EmployeePresenceDTO
    {
        public string Id { get; set; } = "";
        public string FullName { get; set; } = "";
        public int AllDaysCount { get; set; }
        public int DaysPresent { get; set; }
        public int DaysOnVacation { get; set; }
    }
}
