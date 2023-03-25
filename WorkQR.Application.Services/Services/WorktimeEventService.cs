using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WorkQR.Application;
using WorkQR.Data.Abstraction;
using WorkQR.Dictionaries;
using WorkQR.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkQR.Application
{
    public class WorktimeEventService : IWorktimeEventService
    {
        private readonly IWorktimeEventRepository _worktimeEventRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorktimeEventService(IWorktimeEventRepository worktimeEventRepository, UserManager<ApplicationUser> userManager)
        {
            _worktimeEventRepository = worktimeEventRepository;
            _userManager = userManager;
        }

        public async Task<List<WorktimeEventDTO>> GetUserWorktimeEventsToday(string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<WorktimeEvent> worktimeEvents = await _worktimeEventRepository.GetWithConditionAsync(x => x.ApplicationUserId == user.Id
                                                                                                                  && x.EventTime >= DateTime.Today
                                                                                                                  && x.EventTime < DateTime.Today.AddDays(1));
            LinkedList<WorktimeEvent> linkedWorktimeEvents = new(worktimeEvents.OrderByDescending(x => x.EventTime));
            List<WorktimeEventDTO> worktimeEventsList = new();

            for (var element = linkedWorktimeEvents.First; element != null; element = element.Next)
            {
                var worktimeEvent = element.Value;
                worktimeEventsList.Add(new WorktimeEventDTO()
                {
                    Id = worktimeEvent.Id,
                    EventType = worktimeEvent.EventType,
                    EventTypeName = worktimeEvent.EventType.GetEnumDescription(),
                    EventTime = worktimeEvent.EventTime,
                    Description = worktimeEvent.Description,
                    DurationInSecs = worktimeEvent.EventType != EventType.EndWork ? ((element.Previous?.Value.EventTime ?? DateTime.Now) - element.Value.EventTime).TotalSeconds : 0,
                });
            }

            return worktimeEventsList;
        }

        public async Task<WorktimeEventsTimestampsDTO> GetUserWorktimeEventsBetweenDates(DaysSpanVM model, string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<WorktimeEvent> worktimeEvents = await _worktimeEventRepository.GetWithConditionAsync(x => x.ApplicationUserId == user.Id
                                                                                                                  && x.EventTime >= model.DateFrom
                                                                                                                  && x.EventTime <= model.DateTo);
            LinkedList<WorktimeEvent> linkedWorktimeEvents = new(worktimeEvents.OrderBy(x => x.EventTime));
            WorktimeEventsTimestampsDTO worktimeEventsDTO = new();

            for (var element = linkedWorktimeEvents.First; element != null; element = element.Next)
            {
                var worktimeEvent = element.Value;
                worktimeEventsDTO.Timestamps.Add(new QTimestampDTO()
                {
                    Date = worktimeEvent.EventTime.ToString("yyyy-MM-dd"),
                    Time = worktimeEvent.EventTime.ToString("HH:mm"),
                    Title = worktimeEvent.EventType.GetQTimestampTitle(),
                    EventType = worktimeEvent.EventType,
                    Header = worktimeEvent.EventType.GetQTimestampDescription(),
                    Details = worktimeEvent.Description,
                    Duration = worktimeEvent.EventType != EventType.EndWork ? ((element.Next?.Value.EventTime ?? DateTime.Now) - element.Value.EventTime).TotalMinutes : 0
                });
            }

            return worktimeEventsDTO;
        }
    }
}