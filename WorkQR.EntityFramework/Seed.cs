using Microsoft.AspNetCore.Identity;
using WorkQR.Domain;

namespace WorkQR.EntityFramework
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private Company _company1;
        private Position _company1Position1;
        private Position _company1Position2;
        private Position _company1Position3;
        private Position _company1Position4;


        public Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void SeedData()
        {
            await AddCompanies();
            await AddPositions();
            await AddUsers();
        }

        private async Task AddCompanies()
        {
            if (_context.Companies.Any())
            {
                return;
            }

            _company1 = new()
            {
                Name = "Januszex Company",
                Description = "Perspektywiczna firma kurierska z młodym oraz dynamicznym zespołem"
            };

            await _context.Companies.AddAsync(_company1);
            await _context.SaveChangesAsync();
        }

        private async Task AddPositions()
        {
            if (_context.Positions.Any())
            {
                return;
            }

            _company1Position1 = new()
            {
                Name = "Prezes",
                Company = _company1,
                BreakMinsPerDay = 480
            };
            await _context.Positions.AddAsync(_company1Position1);

            _company1Position2 = new()
            {
                Name = "Kierownik",
                Company = _company1,
                BreakMinsPerDay = 60
            };
            await _context.Positions.AddAsync(_company1Position2);

            _company1Position3 = new()
            {
                Name = "Kierowca",
                Company = _company1,
                BreakMinsPerDay = 20
            };
            await _context.Positions.AddAsync(_company1Position3);

            _company1Position4 = new()
            {
                Name = "Magazynier",
                Company = _company1,
                BreakMinsPerDay = 15
            };
            await _context.Positions.AddAsync(_company1Position4);

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
                    Position = _company1Position1
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
                    Position = _company1Position1
                };
                await _userManager.CreateAsync(modUser, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));
                await _userManager.AddToRoleAsync(modUser, UserRoles.Moderator);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                ApplicationUser normalUser = new()
                {
                    FirstName = "Szymon",
                    LastName = "Baran",
                    Email = "sbaran@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "sbaran",
                    Position = _company1Position4,
                    QrAuthorizationKey = Guid.Empty
                };
                await _userManager.CreateAsync(normalUser, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                await _userManager.AddToRoleAsync(normalUser, UserRoles.User);
            }

            await _context.SaveChangesAsync();
        }
    }
}