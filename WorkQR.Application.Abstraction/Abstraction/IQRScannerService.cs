using Microsoft.AspNetCore.Identity;
using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public interface IQRScannerService
    {
        Task<EventType?> Scan(Guid qrAuthorizationKey);
    }
}
