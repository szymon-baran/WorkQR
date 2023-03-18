using WorkQR.Data.Abstraction;
using WorkQR.Dictionaries;
using WorkQR.Domain;

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

            List<WorktimeEvent> worktimeEvents = await GetUserTodaysWorktimeEvents(user.Id);


            EventType newEventType = (worktimeEvents.Any() ? worktimeEvents.Last().EventType : EventType.EndWork).GetDefaultNewEventType();
            double breakMinutesLeftToday = CalculateBreakMinutesLeftToday(worktimeEvents, newEventType, user.Position?.BreakMinsPerDay ?? 0);

            if (newEventType == EventType.StartBreak && breakMinutesLeftToday <= 0)
                throw new Exception($"Dzisiejszy czas przerwy został już wykorzystany! Masz do dyspozycji {user.Position?.BreakMinsPerDay ?? 0} minut przerwy dziennie.");

            WorktimeEvent worktimeEvent = new()
            {
                ApplicationUserId = user.Id,
                EventType = newEventType,
                EventTime = DateTime.Now
            };

            await _worktimeEventRepository.AddAsync(worktimeEvent);
            await _worktimeEventRepository.SaveChangesAsync();

            return new()
            {
                EventType = worktimeEvent.EventType,
                BreakMinutesLeftToday = breakMinutesLeftToday
            };
        }

        private async Task<List<WorktimeEvent>> GetUserTodaysWorktimeEvents(string userId)
        {
            IEnumerable<WorktimeEvent> worktimeEvents = await _worktimeEventRepository.GetWithConditionAsync(x => x.ApplicationUserId == userId && x.EventTime == DateTime.Today);
            if (worktimeEvents.Any())
                return worktimeEvents.OrderBy(x => x.EventTime).ToList();
            else
                return new();
        }

        private double CalculateBreakMinutesLeftToday(List<WorktimeEvent> worktimeEventsToday, EventType newEventType, int userBreakMinutesPerDay)
        {
            double breakMinutesToday = 0;

            foreach (var breakStartEvent in worktimeEventsToday.Where(x => x.EventType == EventType.StartBreak)
                                                            .Select((value, index) => new { index, value }))
            {
                var breakEndEvent = worktimeEventsToday.Skip(breakStartEvent.index).FirstOrDefault(x => x.EventType == EventType.EndBreak);
                if (breakEndEvent != null)
                {
                    breakMinutesToday += (breakEndEvent.EventTime - breakStartEvent.value.EventTime).TotalMinutes;
                }
                else if (newEventType == EventType.EndBreak)
                {
                    breakMinutesToday += (DateTime.Now - breakStartEvent.value.EventTime).TotalMinutes;
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
    }
}