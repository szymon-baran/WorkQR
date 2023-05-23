namespace WorkQR.Application
{
    public class RaportDocumentVM
    {
        public DateTime DateFrom { get; set; } = new();
        public DateTime DateTo { get; set; } = new();
        public List<string> Employees { get; set; } = new();
    }
}
