using Microsoft.AspNetCore.Identity;
using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public interface IQRService
    {
        Task<EventScanDTO> Scan(Guid qrAuthorizationKey);
        Task<Guid> GetQRAuthorizationKeyByUserName(string userName);
    }
}
