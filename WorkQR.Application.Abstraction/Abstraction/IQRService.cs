using Microsoft.AspNetCore.Identity;
using WorkQR.Domain.Dictionaries;

namespace WorkQR.Application
{
    public interface IQRService
    {
        Task<EventScanDTO> Scan(Guid qrAuthorizationKey);
        Task<Guid> GetQRAuthorizationKeyByUserName(string userName);
        Task<Guid> ResetUserQRAuthorizationKey(string userId);
        Task CancelEventById(Guid id);
        Task ChangeEventTypeToEndById(Guid id);
    }
}
