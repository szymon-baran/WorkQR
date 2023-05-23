using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WorkQR.Data.Abstraction;
using WorkQR.Dictionaries;
using WorkQR.Domain;

namespace WorkQR.Application
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;
        public ApplicationUserService(UserManager<ApplicationUser> userManager,
                                      IApplicationUserRepository applicationUserRepository,
                                      IPositionRepository positionRepository,
                                      IMapper mapper)
        {
            _userManager = userManager;
            _applicationUserRepository = applicationUserRepository;
            _positionRepository = positionRepository;
            _mapper = mapper;
        }

        public async Task<List<FullEmployeeDTO>> GetCompanyEmployeesForModerator(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.Position == null)
                throw new Exception("Nie znaleziono użytkownika!");

            IEnumerable<ApplicationUser> applicationUsers = await _applicationUserRepository.GetWithConditionAsync(x => x.Position != null
                                                                                                                        && x.Position.CompanyId == user.Position.CompanyId 
                                                                                                                        && (x.LockoutEnd == null || x.LockoutEnd <= DateTime.Now || x.RegistrationCode == ""));

            return applicationUsers.Select(x => new FullEmployeeDTO()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.UserName ?? "",
                PositionId = x.Position.Id,
                RegistrationCode = x.RegistrationCode ?? "",
                QrAuthorizationKey = x.QrAuthorizationKey,
                LastActivity = x.WorktimeEvents.OrderByDescending(x => x.EventTime).FirstOrDefault()?.EventTime ?? null,
                IsOnVacation = x.Vacations.Any(x => x.IsApproved && x.DateFrom <= DateTime.Now && x.DateTo >= DateTime.Now),
                VacationDaysPerYear = x.VacationDaysPerYear ?? 0,
                VacationDaysThisYearLeft = ((x.VacationDaysPerYear ?? 0 - (int)Math.Round(x.Vacations.Where(x => x.IsApproved && !x.IsRejected && x.DateTo.Year == DateTime.Today.Year && x.VacationType == VacationType.AnnualLeave).Sum(x => (x.DateTo - x.DateFrom).TotalDays))))
            }).OrderByDescending(x => x.IsOnVacation).ThenByDescending(x => x.LastActivity).ToList();
        }

        public async Task<List<SelectDTO<string>>> GetCompanyEmployeesToSelect(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.Position == null)
                throw new Exception("Nie znaleziono użytkownika!");

            IEnumerable<ApplicationUser> applicationUsers = await _applicationUserRepository.GetWithConditionAsync(x => x.Position != null && x.Position.CompanyId == user.Position.CompanyId && x.Position.UserRoleName != "QRScanner");

            return applicationUsers.Select(x => new SelectDTO<string>()
            {
                Value = x.Id,
                Label = $"{x.LastName} {x.FirstName}"
            }).OrderBy(x => x.Label).ToList();
        }

        public async Task<List<FullEmployeeDTO>> GetCompanyInactiveAccounts(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.Position == null)
                throw new Exception("Nie znaleziono użytkownika!");

            IEnumerable<ApplicationUser> applicationUsers = await _applicationUserRepository.GetWithConditionAsync(x => x.Position != null
                                                                                                                        && x.Position.CompanyId == user.Position.CompanyId 
                                                                                                                        && x.LockoutEnd > DateTime.Now
                                                                                                                        && x.RegistrationCode != "");

            return applicationUsers.Select(x => new FullEmployeeDTO()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.UserName ?? "",
                PositionId = x.Position.Id,
                RegistrationCode = x.RegistrationCode ?? "",
                QrAuthorizationKey = x.QrAuthorizationKey,
                LastActivity = x.WorktimeEvents.OrderByDescending(x => x.EventTime).FirstOrDefault()?.EventTime ?? null,
            }).OrderByDescending(x => x.LastActivity).ToList();
        }

        public async Task<List<EmployeeDTO>> GetCompanyEmployees(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.Position == null)
                throw new Exception("Nie znaleziono użytkownika!");

            IEnumerable<ApplicationUser> applicationUsers = await _applicationUserRepository.GetWithConditionAsync(x => (!x.LockoutEnd.HasValue
                                                                                                                        || x.LockoutEnd.Value <= DateTime.Now)
                                                                                                                        && x.Position != null
                                                                                                                        && x.Position.CompanyId == user.Position.CompanyId
                                                                                                                        && x.Position.UserRoleName != "QRScanner");

            return applicationUsers.OrderByDescending(x => x.Position.BreakMinsPerDay).Select(x => new EmployeeDTO()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.UserName ?? "",
                PositionName = x.Position?.Name ?? ""
            }).ToList();
        }

        public async Task UpdateCompanyEmployees(string username, List<CompanyEmployeeVM> model)
        {
            var loggedUser = await _userManager.FindByNameAsync(username);

            if (loggedUser == null || loggedUser.Position == null)
                throw new Exception("Nie znaleziono użytkownika!");

            IEnumerable<ApplicationUser> applicationUsers = await _applicationUserRepository.GetWithConditionAsync(x => x.Position != null && x.Position.CompanyId == loggedUser.Position.CompanyId);
            applicationUsers = applicationUsers.Where(x => model.Any(y => y.Id == x.Id)).ToList();
            foreach (var user in applicationUsers)
            {
                CompanyEmployeeVM modelEmployee = model.First(y => y.Id == user.Id);
                if (modelEmployee.Username != user.UserName)
                {
                    await _userManager.SetUserNameAsync(user, modelEmployee.Username);
                    await _userManager.UpdateNormalizedUserNameAsync(user);
                }
                if (modelEmployee.IsDisabled)
                {
                    user.LockoutEnd = DateTime.MaxValue;
                    user.LockoutEnabled = true;
                }
                else
                {
                    user.LockoutEnd = null;
                    user.LockoutEnabled = false;
                }
                if (modelEmployee.PositionId != user.PositionId)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    Position position = await _positionRepository.FirstOrDefaultAsync(x => x.Id == modelEmployee.PositionId);
                    if (position != null)
                    {
                        await _userManager.AddToRoleAsync(user, position.UserRoleName);
                    }
                    user.PositionId = modelEmployee.PositionId;
                }
                user.FirstName = modelEmployee.FirstName;
                user.LastName = modelEmployee.LastName;
                user.PositionId = modelEmployee.PositionId;
                user.VacationDaysPerYear = modelEmployee.VacationDaysPerYear;
            }

            await _applicationUserRepository.SaveChangesAsync();
        }

        public bool IsUserDisabled(ApplicationUser user)
        {
            return user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now;
        }

    }
}