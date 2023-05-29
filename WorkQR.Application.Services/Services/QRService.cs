using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Dictionaries;
using WorkQR.Domain.Models;

namespace WorkQR.Application
{
    public class QRService : IQRService
    {
        private readonly IWorktimeEventRepository _worktimeEventRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public QRService(IWorktimeEventRepository worktimeEventRepository, IApplicationUserRepository applicationUserRepository)
        {
            _worktimeEventRepository = worktimeEventRepository;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<EventScanDTO> Scan(Guid qrAuthorizationKey)
        {
            var user = await _applicationUserRepository.FirstOrDefaultAsync(x => x.QrAuthorizationKey == qrAuthorizationKey);

            if (user == null)
                throw new Exception("Nie znaleziono użytkownika!");

            EventScanDTO dto = new()
            {
                FullName = $"{user.FirstName} {user.LastName}",
                IsOnVacation = false,
                VacationTo = null
            };

            Vacation currentVacation = user.Vacations.FirstOrDefault(x => x.DateFrom.Date <= DateTime.Now.Date && x.DateTo.Date >= DateTime.Now.Date && x.IsApproved && !x.IsRejected);

            if (currentVacation != null)
            {
                dto.IsOnVacation = true;
                dto.VacationTo = currentVacation.DateTo;
            }       

            List<WorktimeEvent> worktimeEvents = await GetUserTodaysWorktimeEvents(user.Id);


            EventType newEventType = (worktimeEvents.Any() ? worktimeEvents.Last().EventType : EventType.EndWork).GetDefaultNewEventType();
            double breakMinutesLeftToday = CalculateBreakMinutesLeftToday(worktimeEvents, newEventType, user.Position?.BreakMinsPerDay ?? 0);

            if (breakMinutesLeftToday < 1)
            {
                newEventType = EventType.EndWork;
            }

            //if (newEventType == EventType.StartBreak && breakMinutesLeftToday <= 0)
            //    throw new Exception($"Dzisiejszy czas przerwy został już wykorzystany! Masz do dyspozycji {user.Position?.BreakMinsPerDay ?? 0} minut przerwy dziennie.");

            WorktimeEvent worktimeEvent = new()
            {
                ApplicationUserId = user.Id,
                EventType = newEventType,
                EventTime = DateTime.Now
            };

            await _worktimeEventRepository.AddAsync(worktimeEvent);
            await _worktimeEventRepository.SaveChangesAsync();

            dto.Id = worktimeEvent.Id;
            dto.EventType = worktimeEvent.EventType;
            dto.BreakMinutesLeftToday = breakMinutesLeftToday;

            return dto;
        }

        private async Task<List<WorktimeEvent>> GetUserTodaysWorktimeEvents(string userId)
        {
            IEnumerable<WorktimeEvent> worktimeEvents = await _worktimeEventRepository.GetWithConditionAsync(x => x.ApplicationUserId == userId && x.EventTime.Date == DateTime.Today);
            if (worktimeEvents.Any())
                return worktimeEvents.OrderBy(x => x.EventTime).ToList();
            else
                return new();
        }

        private double CalculateBreakMinutesLeftToday(List<WorktimeEvent> worktimeEventsToday, EventType newEventType, int userBreakMinutesPerDay)
        {
            double breakMinutesToday = 0;

            foreach (var breakStartEvent in worktimeEventsToday.Where(x => x.EventType == EventType.StartBreak))
            {
                var index = worktimeEventsToday.IndexOf(breakStartEvent);
                var breakEndEvent = worktimeEventsToday.Skip(index).FirstOrDefault(x => x.EventType == EventType.EndBreak);
                if (breakEndEvent != null)
                {
                    breakMinutesToday += (breakEndEvent.EventTime - breakStartEvent.EventTime).TotalMinutes;
                }
                else if (newEventType == EventType.EndBreak)
                {
                    breakMinutesToday += (DateTime.Now - breakStartEvent.EventTime).TotalMinutes;
                }
            }

            return userBreakMinutesPerDay - breakMinutesToday;
        }

        public async Task<Guid> GetQRAuthorizationKeyByUserName(string userName)
        {
            var user = await _applicationUserRepository.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null)
                throw new Exception("Nie znaleziono użytkownika!");

            return user.QrAuthorizationKey;
        }

        public async Task<Guid> ResetUserQRAuthorizationKey(string userId)
        {
            var user = await _applicationUserRepository.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new Exception("Nie znaleziono użytkownika!");

            user.QrAuthorizationKey = Guid.NewGuid();
            _applicationUserRepository.Update(user);
            await _applicationUserRepository.SaveChangesAsync();

            return user.QrAuthorizationKey;
        }

        public async Task CancelEventById(Guid id)
        {
            var worktimeEvent = await _worktimeEventRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (worktimeEvent != null)
            {
                _worktimeEventRepository.Remove(worktimeEvent);
                await _worktimeEventRepository.SaveChangesAsync();
            }
        }

        public async Task ChangeEventTypeToEndById(Guid id)
        {
            var worktimeEvent = await _worktimeEventRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (worktimeEvent != null)
            {
                worktimeEvent.EventType = EventType.EndWork;
                _worktimeEventRepository.Update(worktimeEvent);
                await _worktimeEventRepository.SaveChangesAsync();
            }
        }
    }
}