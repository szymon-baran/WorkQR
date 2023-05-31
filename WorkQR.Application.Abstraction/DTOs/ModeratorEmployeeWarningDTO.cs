namespace WorkQR.Application
{
    public class ModeratorEmployeeWarningDTO
    {
        public string Id { get; set; } = "";
        public string FullName { get; set; }
        public int OverextendedBreaksCount { get; set; }
        public int NotEnoughDailyWorkCount { get; set; }
    }
}
