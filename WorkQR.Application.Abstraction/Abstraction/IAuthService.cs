using Microsoft.AspNetCore.Identity;
using WorkQR.Domain.Models;

namespace WorkQR.Application
{
    public interface IAuthService
    {
        Task<UserDTO> LoginAsync(UserLoginVM model);
        Task<CompanyRegisterResultDTO> CompanyRegisterAsync(CompanyRegisterVM model);
        Task<UserTokenDTO> RefreshAccessTokenAsync(UserTokenVM model);
        Task<bool> ValidateUsername(string username);
        Task<RegistrationCodeUserDTO> GetUserDataByRegistrationCode(string registrationCode);
        Task AddEmployee(EmployeeAddVM model);
        Task ActivateEmployee(EmployeeActivateVM model);
    }
}
