using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WorkQR.Application;
using WorkQR.Data.Abstraction;
using WorkQR.Domain;

namespace WorkQR.Application
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UserDTO> LoginAsync(UserLoginVM model)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                throw new Exception("Nie znaleziono użytkownika!");

            var isSuccess = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isSuccess)
                throw new Exception("Niepoprawne hasło!");

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);
            var refreshToken = GetRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);
            await _userManager.UpdateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            UserDTO userDTO = new()
            {
                Username = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = new DateTimeOffset(token.ValidTo).ToUnixTimeMilliseconds(),
                Roles = roles.ToList()
            };

            return userDTO;
        }

        public async Task<IdentityResult> RegisterAsync(UserRegisterVM model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                throw new Exception("Istnieje już użytkownik o podanej nazwie!");

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        public async Task<UserTokenDTO> RefreshAccessTokenAsync(UserTokenVM model)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(model.AccessToken, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Błędny token.");

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new Exception("Wymagane ponowne zalogowanie.");

            var newAccessToken = GetToken(principal.Claims.ToList());
            var newRefreshToken = GetRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new UserTokenDTO
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                Expiration = new DateTimeOffset(newAccessToken.ValidTo).ToUnixTimeMilliseconds(),
                RefreshToken = newRefreshToken
            };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(6),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        private string GetRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}