using WorkQR.Domain.Models;

namespace WorkQR.Application
{
    public interface IApplicationUserService
    {
        Task<List<FullEmployeeDTO>> GetCompanyEmployeesForModerator(string username);
        Task<List<SelectDTO<string>>> GetCompanyEmployeesToSelect(string username);
        Task<List<FullEmployeeDTO>> GetCompanyInactiveAccounts(string username);
        Task<List<EmployeeDTO>> GetCompanyEmployees(string username);
        Task UpdateCompanyEmployees(string username, List<CompanyEmployeeVM> model);
        bool IsUserDisabled(ApplicationUser user);
    }
}
