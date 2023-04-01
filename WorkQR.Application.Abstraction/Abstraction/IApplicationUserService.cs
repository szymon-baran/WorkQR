using Microsoft.AspNetCore.Identity;
using WorkQR.Dictionaries;
using WorkQR.Domain;

namespace WorkQR.Application
{
    public interface IApplicationUserService
    {
        Task<List<EmployeeDTO>> GetCompanyEmployees(string username);
        bool IsUserDisabled(ApplicationUser user);
    }
}
