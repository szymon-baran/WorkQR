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

        public ApplicationUserService(UserManager<ApplicationUser> userManager, IApplicationUserRepository applicationUserRepository)
        {
            _userManager = userManager;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<List<EmployeeDTO>> GetCompanyEmployees(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.Position == null)
                throw new Exception("Nie znaleziono użytkownika!");

            IEnumerable<ApplicationUser> applicationUsers = await _applicationUserRepository.GetWithConditionAsync(x => x.Position != null
                                                                                                                        && x.Position.CompanyId == user.Position.CompanyId);

            return applicationUsers.OrderByDescending(x => x.Position.BreakMinsPerDay).Select(x => new EmployeeDTO()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.UserName ?? "",
                PositionName = x.Position.Name,
                RegistrationCode = x.RegistrationCode ?? "",
                IsDisabled = IsUserDisabled(x)
            }).ToList();
        }

        public bool IsUserDisabled(ApplicationUser user)
        {
            return user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now;
        }

    }
}