using Microsoft.AspNetCore.Identity;

namespace WorkQR.Application
{
    public class CompanyRegisterResultDTO
    {
        public IdentityResult? ModeratorResult { get; set; }
        public IdentityResult? ScannerResult { get; set; }
        public string ScannerUsername { get; set; } = "";
        public string ScannerPassword { get; set; } = "";
    }
}
