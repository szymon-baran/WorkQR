using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WorkQR.Application;
using WorkQR.Data.Abstraction;
using WorkQR.Dictionaries;
using WorkQR.Domain;

namespace WorkQR.Application
{
    public class QRScannerService : IQRScannerService
    {
        private readonly IWorktimeEventRepository _worktimeEventRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public QRScannerService(IWorktimeEventRepository worktimeEventRepository, IApplicationUserRepository applicationUserRepository)
        {
            _worktimeEventRepository = worktimeEventRepository;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<EventType?> Scan(Guid qrAuthorizationKey)
        {
            var user = await _applicationUserRepository.FirstOrDefaultAsync(x => x.QrAuthorizationKey == qrAuthorizationKey);

            if (user == null)
                return null;

            WorktimeEvent? latestWorktimeEvent = await GetUserLatestWorktimeEvent(user.Id);
            EventType newEventType = (latestWorktimeEvent?.EventType ?? EventType.EndWork).GetDefaultNewEventType();
            WorktimeEvent worktimeEvent = new()
            {
                ApplicationUserId = user.Id,
                EventType = newEventType,
                EventTime = DateTime.Now
            };

            await _worktimeEventRepository.AddAsync(worktimeEvent);
            await _worktimeEventRepository.SaveChangesAsync();

            return worktimeEvent.EventType;
        }

        private async Task<WorktimeEvent?> GetUserLatestWorktimeEvent(string userId)
        {
            IEnumerable<WorktimeEvent> worktimeEvents = await _worktimeEventRepository.GetWithConditionAsync(x => x.ApplicationUserId == userId);
            if (worktimeEvents.Any())
                return worktimeEvents.OrderByDescending(x => x.EventTime).FirstOrDefault();
            else
                return null;
        }
    }
}