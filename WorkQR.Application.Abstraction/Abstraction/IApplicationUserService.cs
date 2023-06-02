using WorkQR.Domain.Models;

namespace WorkQR.Application
{
    public interface IApplicationUserService
    {
        Task<List<FullEmployeeDTO>> GetCompanyEmployeesForModerator();
        Task<List<SelectDTO<string>>> GetCompanyEmployeesToSelect();
        Task<List<FullEmployeeDTO>> GetCompanyInactiveAccounts();
        Task<List<EmployeeDTO>> GetCompanyEmployees();
        Task UpdateCompanyEmployees(List<CompanyEmployeeVM> model);
        bool IsUserDisabled(ApplicationUser user);
    }
}
