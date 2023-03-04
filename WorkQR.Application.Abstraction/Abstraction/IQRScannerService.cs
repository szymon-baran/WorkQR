using Microsoft.AspNetCore.Identity;
using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public interface IQRScannerService
    {
        Task<EventScanDTO> Scan(Guid qrAuthorizationKey);
    }
}
