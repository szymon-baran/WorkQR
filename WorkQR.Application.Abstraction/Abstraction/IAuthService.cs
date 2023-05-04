using Microsoft.AspNetCore.Identity;
using WorkQR.Domain;

namespace WorkQR.Application
{
    public interface IAuthService
    {
        Task<UserDTO> LoginAsync(UserLoginVM model);
        Task<CompanyRegisterResultDTO> CompanyRegisterAsync(CompanyRegisterVM model);
        Task<UserTokenDTO> RefreshAccessTokenAsync(UserTokenVM model);
        Task<bool> ValidateUsername(string username);
        Task AddEmployee(EmployeeAddVM model, string userName);
    }
}
