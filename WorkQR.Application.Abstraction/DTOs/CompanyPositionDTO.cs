namespace WorkQR.Application
{
    public class CompanyPositionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double TimeBasis { get; set; }
        public int BreakMinsPerDay { get; set; }
        public Guid? CompanyId { get; set; }
        public bool IsSystemPosition { get; set; }
        public string UserRoleName { get; set; } = "";
    }
}
