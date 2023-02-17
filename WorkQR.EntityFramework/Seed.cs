using Microsoft.AspNetCore.Identity;
using WorkQR.Domain;

namespace WorkQR.EntityFramework
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private Company company1;


        public Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void SeedData()
        {
            await AddCompanies();
            await AddUsers();
        }

        private async Task AddCompanies()
        {
            if (_context.Companies.Any())
            {
                return;
            }

            company1 = new()
            {
                Name = "Januszex",
                Description = "Perspektywiczna firma z młodym oraz dynamicznym zespołem"
            };

            await _context.Companies.AddAsync(company1);
            await _context.SaveChangesAsync();
        }


        private async Task AddUsers()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                ApplicationUser adminUser = new()
                {
                    Email = "admin@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "admin"
                };
                await _userManager.CreateAsync(adminUser, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.QRScanner))
            {
                ApplicationUser scannerUser = new()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "januszexscanner",
                    Company = company1
                };
                await _userManager.CreateAsync(scannerUser, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.QRScanner));
                await _userManager.AddToRoleAsync(scannerUser, UserRoles.QRScanner);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Moderator))
            {
                ApplicationUser modUser = new()
                {
                    FirstName = "Janusz",
                    LastName = "Kowalski",
                    Email = "jkowalski@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "jkowalski",
                    Company = company1
                };
                await _userManager.CreateAsync(modUser, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));
                await _userManager.AddToRoleAsync(modUser, UserRoles.Moderator);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                ApplicationUser normalUser = new()
                {
                    FirstName = "Stefan",
                    LastName = "Klocek",
                    Email = "sklocek@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "sklocek",
                    Company = company1
                };
                await _userManager.CreateAsync(normalUser, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                await _userManager.AddToRoleAsync(normalUser, UserRoles.User);
            }

            await _context.SaveChangesAsync();
        }
    }
}