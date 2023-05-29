using Microsoft.AspNetCore.Identity;
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

            return FromWorktimeEventsToLinkedDTO(worktimeEvents);
        }

        public async Task<List<WorktimeEventDTO>> GetEmployeeWorkHours(GetUserDetailsVM model)
        {
            IEnumerable<WorktimeEvent> worktimeEvents = await _worktimeEventRepository.GetWorktimeEvents(model);
            return FromWorktimeEventsToLinkedDTO(worktimeEvents);
        }

        // TODO: move to converter
        private List<WorktimeEventDTO> FromWorktimeEventsToLinkedDTO(IEnumerable<WorktimeEvent> worktimeEvents)
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
                                                                                                                  && x.EventTime.Date >= model.DateFrom.Date
                                                                                                                  && x.EventTime.Date <= model.DateTo.Date);
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

        public async Task<byte[]> GetCompanyRaportForDate(ModeratorRaportDocumentVM model, string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<ApplicationUser> employees = await _applicationUserRepository.GetWithConditionAsync(x => model.Employees.Contains(x.Id));
            
            List<RaportEmployeeDTO> employeesDTO = new();
            foreach (var employee in employees.OrderBy(x => x.LastName).ThenBy(x => x.FirstName))
            {
                double workedMinutes = 0;
                LinkedList<WorktimeEvent> linkedWorktimeEvents = new(employee.WorktimeEvents.Where(x => x.EventTime.Date >= model.DateFrom.Date && x.EventTime.Date <= model.DateTo.Date).OrderBy(x => x.EventTime));
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

        public async Task<EmployeePresenceDTO> GetEmployeePresenceData(RaportDocumentVM model, string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            var daysSpan = Enumerable.Range(0, 1 + model.DateTo.Subtract(model.DateFrom).Days).Select(offset => model.DateFrom.AddDays(offset)).Count();

            return new()
            {
                Id = user.Id,
                DaysPresent = user.WorktimeEvents.Where(x => x.EventType == EventType.StartWork && x.EventTime.Date >= model.DateFrom.Date && x.EventTime.Date <= model.DateTo.Date)
                                              .DistinctBy(x => x.EventTime.Date).Count(),
                DaysOnVacation = (int)Math.Round(user.Vacations.Where(x => x.DateFrom.Date <= model.DateTo.Date && model.DateFrom.Date <= x.DateTo.Date).Select(x => (x.DateTo - x.DateFrom).TotalDays).Sum()),
                AllDaysCount = daysSpan
            };
        }

        public async Task<EmployeeWorkTimeComparisonDTO> GetEmployeeWorkTimeComparisonData(RaportDocumentVM model, string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<ApplicationUser> employees = await _applicationUserRepository.GetWithConditionAsync(x => x.Position.CompanyId == user.Position.CompanyId);

            List<ModeratorEmployeeWorkedHoursDTO> workedHoursDTOList = new();

            foreach (var employee in employees)
            {
                LinkedList<WorktimeEvent> linkedWorktimeEvents = new(employee.WorktimeEvents.Where(x => x.EventTime.Date >= model.DateFrom.Date
                                                                                                    && x.EventTime.Date <= model.DateTo.Date).OrderBy(x => x.EventTime));

                ModeratorEmployeeWorkedHoursDTO workedHoursDTO = new()
                {
                    Id = employee.Id,
                    PositionId = employee.PositionId,
                    FullName = $"{employee.LastName} {employee.FirstName}"
                };

                for (var element = linkedWorktimeEvents.First; element != null; element = element.Next)
                {
                    var worktimeEvent = element.Value;
                    if (worktimeEvent.EventType != EventType.EndWork)
                    {
                        if (worktimeEvent.EventType == EventType.StartWork || worktimeEvent.EventType == EventType.EndBreak)
                        {
                            workedHoursDTO.WorkedHours += ((element.Next?.Value.EventTime ?? DateTime.Now) - element.Value.EventTime).TotalHours;
                        }
                        else
                        {
                            workedHoursDTO.BreakHours += ((element.Next?.Value.EventTime ?? DateTime.Now) - element.Value.EventTime).TotalHours;
                        }
                    }
                }

                workedHoursDTO.WorkedHours = Math.Round(workedHoursDTO.WorkedHours, 2);
                workedHoursDTO.BreakHours = Math.Round(workedHoursDTO.BreakHours, 2);

                workedHoursDTOList.Add(workedHoursDTO);
            }

            return new()
            {
                WorkedHours = workedHoursDTOList.First(x => x.Id == user.Id).WorkedHours,
                BreakHours = workedHoursDTOList.First(x => x.Id == user.Id).BreakHours,
                EveryoneWorkedHours = Math.Round(workedHoursDTOList.Average(x => x.WorkedHours), 2),
                EveryoneBreakHours = Math.Round(workedHoursDTOList.Average(x => x.BreakHours), 2),
                PositionWorkedHours = Math.Round(workedHoursDTOList.Where(x => x.PositionId == user.PositionId).Average(x => x.WorkedHours), 2),
                PositionBreakHours = Math.Round(workedHoursDTOList.Where(x => x.PositionId == user.PositionId).Average(x => x.BreakHours), 2),
            };
        }

        public async Task<List<EmployeePresenceDTO>> GetModeratorEmployeesPresenceData(ModeratorRaportDocumentVM model, string userName)
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
                DaysPresent = x.WorktimeEvents.Where(x => x.EventType == EventType.StartWork && x.EventTime.Date >= model.DateFrom.Date && x.EventTime.Date <= model.DateTo.Date)
                                              .DistinctBy(x => x.EventTime.Date).Count(),
                DaysOnVacation = (int)Math.Round(x.Vacations.Where(x => x.DateFrom.Date <= model.DateTo.Date && model.DateFrom.Date <= x.DateTo.Date).Select(x => (x.DateTo - x.DateFrom).TotalDays).Sum()),
                AllDaysCount = daysSpan
            }).ToList();
        }

        public async Task<List<ModeratorEmployeeWorkedHoursDTO>> GetEmployeesWorkedHoursData(ModeratorRaportDocumentVM model, string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            IEnumerable<ApplicationUser> employees = await _applicationUserRepository.GetWithConditionAsync(x => model.Employees.Contains(x.Id));
            List<ModeratorEmployeeWorkedHoursDTO> workedHoursDTOList = new();

            foreach (var employee in employees)
            {
                LinkedList<WorktimeEvent> linkedWorktimeEvents = new(employee.WorktimeEvents.Where(x => x.EventTime.Date >= model.DateFrom.Date
                                                                                                    && x.EventTime.Date <= model.DateTo.Date).OrderBy(x => x.EventTime));

                ModeratorEmployeeWorkedHoursDTO workedHoursDTO = new()
                {
                    Id = employee.Id,
                    FullName = $"{employee.LastName} {employee.FirstName}"
                };

                for (var element = linkedWorktimeEvents.First; element != null; element = element.Next)
                {
                    var worktimeEvent = element.Value;
                    if (worktimeEvent.EventType != EventType.EndWork) 
                    {
                        if (worktimeEvent.EventType == EventType.StartWork || worktimeEvent.EventType == EventType.EndBreak)
                        {
                            workedHoursDTO.WorkedHours += ((element.Next?.Value.EventTime ?? DateTime.Now) - element.Value.EventTime).TotalHours;
                        }
                        else
                        {
                            workedHoursDTO.BreakHours += ((element.Next?.Value.EventTime ?? DateTime.Now) - element.Value.EventTime).TotalHours;
                        }
                    }
                }

                workedHoursDTO.WorkedHours = Math.Round(workedHoursDTO.WorkedHours, 2);
                workedHoursDTO.BreakHours = Math.Round(workedHoursDTO.BreakHours, 2);

                workedHoursDTOList.Add(workedHoursDTO);
            }

            return workedHoursDTOList;
        }

        public async Task UpdateTodayEventDescription(string userName, WorktimeEventTodayEditVM model)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception("Nie znaleziono zalogowanego użytkownika!");

            WorktimeEvent worktimeEvent = await _worktimeEventRepository.FirstOrDefaultAsync(x => x.Id == model.Id) ?? throw new KeyNotFoundException("Brak danych");
            worktimeEvent.Description = model.Description;

            _worktimeEventRepository.Update(worktimeEvent);
            await _worktimeEventRepository.SaveChangesAsync();
        }
    }
}