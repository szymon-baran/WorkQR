using Microsoft.AspNetCore.Identity;

namespace WorkQR.Application
{
    public interface IAuthService
    {
        Task<UserDTO> LoginAsync(UserLoginVM model);
        Task<IdentityResult> RegisterAsync(UserRegisterVM model);
        Task<CompanyRegisterResultDTO> CompanyRegisterAsync(CompanyRegisterVM model);
        Task<UserTokenDTO> RefreshAccessTokenAsync(UserTokenVM model);
        Task<bool> ValidateUsername(string username);
    }
}
