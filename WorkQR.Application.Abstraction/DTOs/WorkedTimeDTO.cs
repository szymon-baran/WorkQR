namespace WorkQR.Application
{
    public class WorkedTimeDTO
    {
        public DateTime DateTime { get; set; }
        public double WorkedHours { get; set; } = 0;
        public double BreakMinutes { get; set; } = 0;
    }
}
