using Microsoft.AspNetCore.Identity;
using WorkQR.Dictionaries;
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
        private ApplicationUser _company1Position4User1;


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
            await AddWorktimeEvents();
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
                Name = "Moderator",
                Company = _company1,
                BreakMinsPerDay = 480,
                IsSystemPosition = true,
                UserRoleName = UserRoles.Moderator
            };
            await _context.Positions.AddAsync(_company1Position1);

            _company1Position2 = new()
            {
                Name = "Skaner QR",
                Company = _company1,
                BreakMinsPerDay = 1000,
                IsSystemPosition = true,
                UserRoleName = UserRoles.QRScanner
            };
            await _context.Positions.AddAsync(_company1Position2);

            _company1Position3 = new()
            {
                Name = "Kierowca",
                Company = _company1,
                BreakMinsPerDay = 20,
                IsSystemPosition = false,
                UserRoleName = UserRoles.User
            };
            await _context.Positions.AddAsync(_company1Position3);

            _company1Position4 = new()
            {
                Name = "Pracownik",
                Company = _company1,
                BreakMinsPerDay = 15,
                IsSystemPosition = false,
                UserRoleName = UserRoles.User
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
                    Position = _company1Position2
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
                    Position = _company1Position1,
                    VacationDaysPerYear = 26
                };
                await _userManager.CreateAsync(modUser, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));
                await _userManager.AddToRoleAsync(modUser, UserRoles.Moderator);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                _company1Position4User1 = new()
                {
                    FirstName = "Szymon",
                    LastName = "Baran",
                    Email = "sbaran@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "sbaran",
                    Position = _company1Position4,
                    QrAuthorizationKey = Guid.Empty,
                    VacationDaysPerYear = 20
                };
                await _userManager.CreateAsync(_company1Position4User1, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                await _userManager.AddToRoleAsync(_company1Position4User1, UserRoles.User);
            }

            await _context.SaveChangesAsync();
        }

        private async Task AddWorktimeEvents()
        {
            if (_context.WorktimeEvents.Any())
            {
                return;
            }
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartWork,
                EventTime = DateTime.Now.AddHours(-8),
                Description = "Przegląd maili, spotkanie z zespołem, praca z projektem"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Now.AddHours(-6),
                Description = "Pierwsza przerwa"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Now.AddHours(-5).AddMinutes(-45),
                Description = "Powrót do prac nad projektem"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Now.AddHours(-2),
                Description = "Druga przerwa"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Now.AddHours(-1).AddMinutes(-40),
                Description = "Spotkanie z klientem"
            });
            //await _context.WorktimeEvents.AddAsync(new()
            //{
            //    ApplicationUser = _company1Position4User1,
            //    EventType = EventType.EndWork,
            //    EventTime = DateTime.Now,
            //    Description = "Koniec pracy"
            //});

            await _context.SaveChangesAsync();
        }
    }
}