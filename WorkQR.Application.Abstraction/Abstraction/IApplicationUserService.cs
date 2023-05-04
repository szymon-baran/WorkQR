using WorkQR.Domain;

namespace WorkQR.Application
{
    public interface IApplicationUserService
    {
        Task<List<FullEmployeeDTO>> GetCompanyEmployeesForModerator(string username);
        Task<List<EmployeeDTO>> GetCompanyEmployees(string username);
        Task UpdateCompanyEmployees(string username, List<CompanyEmployeeVM> model);
        bool IsUserDisabled(ApplicationUser user);
    }
}
