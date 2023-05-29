namespace WorkQR.Application
{
    public class ModeratorEmployeeWorkedHoursDTO
    {
        public string Id { get; set; } = "";
        public Guid? PositionId { get; set; }
        public string FullName { get; set; } = "";
        public double WorkedHours { get; set; }
        public double BreakHours { get; set; }
    }
}
