﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;
using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Dictionaries;
using WorkQR.Domain.Models;

namespace WorkQR.Application
{
    public class WorktimeEventService : IWorktimeEventService
    {
        private readonly IWorktimeEventRepository _worktimeEventRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorktimeEventService(IWorktimeEventRepository worktimeEventRepository, IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager)
        {
            _worktimeEventRepository = worktimeEventRepository;
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
        }

        public async Task<List<WorktimeEventDTO>> GetUserWorktimeEventsToday(string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<WorktimeEvent> worktimeEvents = await _worktimeEventRepository.GetWorktimeEvents(new()
            {
                UserId = user.Id,
                DateFrom = DateTime.Today,
                DateTo = DateTime.Today.AddDays(1),
                Description = null
            });

            return await FromWorktimeEventsToLinkedDTO(worktimeEvents);
        }

        public async Task<List<WorktimeEventDTO>> GetEmployeeWorkHours(GetEventsVM model)
        {
            IEnumerable<WorktimeEvent> worktimeEvents = await _worktimeEventRepository.GetWorktimeEvents(model);
            return await FromWorktimeEventsToLinkedDTO(worktimeEvents);
        }

        // TODO: move to converter
        private async Task<List<WorktimeEventDTO>> FromWorktimeEventsToLinkedDTO(IEnumerable<WorktimeEvent> worktimeEvents)
        {
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

        public async Task<WorktimeEventsTimestampsDTO> GetUserWorktimeEventsBetweenDatesForCalendar(DaysSpanVM model, string userName)
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

        public async Task<byte[]> GetCompanyRaportForDate(RaportDocumentVM model, string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<ApplicationUser> employees = await _applicationUserRepository.GetWithConditionAsync(x => model.Employees.Contains(x.Id));
            
            List<RaportEmployeeDTO> employeesDTO = new();
            foreach (var employee in employees.OrderBy(x => x.LastName).ThenBy(x => x.FirstName))
            {
                double workedMinutes = 0;
                LinkedList<WorktimeEvent> linkedWorktimeEvents = new(employee.WorktimeEvents.Where(x => x.EventTime >= model.DateFrom && x.EventTime <= model.DateTo).OrderBy(x => x.EventTime));
                for (var element = linkedWorktimeEvents.First; element != null; element = element.Next)
                {
                    var worktimeEvent = element.Value;
                    workedMinutes += worktimeEvent.EventType != EventType.EndWork ? ((element.Next?.Value.EventTime ?? DateTime.Now) - element.Value.EventTime).TotalMinutes : 0;
                }
                employeesDTO.Add(new RaportEmployeeDTO()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    WorkedHours = double.Round(workedMinutes / 60, 2)
                });
            }

            RaportDocument raportDocument = new(model, employeesDTO, user.Position.Company.Name);

            byte[] bytes = raportDocument.GeneratePdf();
            return bytes;
        }

        public async Task<List<EmployeePresenceDTO>> GetEmployeesPresenceData(RaportDocumentVM model, string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<ApplicationUser> employees = await _applicationUserRepository.GetWithConditionAsync(x => model.Employees.Contains(x.Id));
            var daysSpan = Enumerable.Range(0, 1 + model.DateTo.Subtract(model.DateFrom).Days).Select(offset => model.DateFrom.AddDays(offset)).Count();

            return employees.Select(x => new EmployeePresenceDTO()
            {
                Id = x.Id,
                FullName = $"{x.LastName} {x.FirstName}",
                DaysPresent = x.WorktimeEvents.Where(x => x.EventType == EventType.StartWork && x.EventTime >= model.DateFrom && x.EventTime <= model.DateTo)
                                              .DistinctBy(x => x.EventTime.Date).Count(),
                DaysOnVacation = (int)Math.Round(x.Vacations.Where(x => x.DateFrom <= model.DateTo && model.DateFrom <= x.DateTo).Select(x => (x.DateTo - x.DateFrom).TotalDays).Sum()),
                AllDaysCount = daysSpan,
                //DaysAbsent = daysSpan.Except(x.WorktimeEvents.Where(x => x.EventType == EventType.StartWork && x.EventTime >= model.DateFrom && x.EventTime <= model.DateTo)
                //                              .DistinctBy(x => x.EventTime.Date).Select(x => x.EventTime.Date)).Count(),
            }).ToList();
        }
    }
}