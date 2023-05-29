using Microsoft.AspNetCore.Identity;
using System;
using WorkQR.Domain.Dictionaries;
using WorkQR.Domain.Models;

namespace WorkQR.Infrastructure.EntityFramework
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
        private ApplicationUser _company1Position3User1;
        private ApplicationUser _company1Position3User2;
        private ApplicationUser _company1Position4User1;
        private ApplicationUser _company1Position4User2;
        private ApplicationUser _company1Position4User3;


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
            await AddVacations();
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
                _company1Position3User1 = new()
                {
                    FirstName = "Szymon",
                    LastName = "Baran",
                    Email = "sbaran@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "sbaran",
                    Position = _company1Position3,
                    QrAuthorizationKey = Guid.Empty,
                    VacationDaysPerYear = 20
                };
                await _userManager.CreateAsync(_company1Position3User1, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                await _userManager.AddToRoleAsync(_company1Position3User1, UserRoles.User);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                _company1Position3User2 = new()
                {
                    FirstName = "Jakub",
                    LastName = "Kubica",
                    Email = "jkubica@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "jkubica",
                    Position = _company1Position3,
                    QrAuthorizationKey = Guid.Empty,
                    VacationDaysPerYear = 26
                };
                await _userManager.CreateAsync(_company1Position3User2, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                await _userManager.AddToRoleAsync(_company1Position3User2, UserRoles.User);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                _company1Position4User1 = new()
                {
                    FirstName = "Mariusz",
                    LastName = "Przykładowski",
                    Email = "mprzykladowski@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "mprzykladowski",
                    Position = _company1Position4,
                    QrAuthorizationKey = Guid.Empty,
                    VacationDaysPerYear = 20
                };
                await _userManager.CreateAsync(_company1Position4User1, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                await _userManager.AddToRoleAsync(_company1Position4User1, UserRoles.User);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                _company1Position4User2 = new()
                {
                    FirstName = "Krzysztof",
                    LastName = "Zwyczajny",
                    Email = "kzwyczajny@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "kzwyczajny",
                    Position = _company1Position4,
                    QrAuthorizationKey = Guid.Empty,
                    VacationDaysPerYear = 26
                };
                await _userManager.CreateAsync(_company1Position4User2, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                await _userManager.AddToRoleAsync(_company1Position4User2, UserRoles.User);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                _company1Position4User3 = new()
                {
                    FirstName = "Ludwik",
                    LastName = "Słoneczny",
                    Email = "lsloneczny@workqr.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "lsloneczny",
                    Position = _company1Position4,
                    QrAuthorizationKey = Guid.Empty,
                    VacationDaysPerYear = 20
                };
                await _userManager.CreateAsync(_company1Position4User3, "Admin1!");
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                await _userManager.AddToRoleAsync(_company1Position4User3, UserRoles.User);
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
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-4).AddHours(10).AddMinutes(3),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-4).AddHours(13).AddMinutes(30),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-4).AddHours(13).AddMinutes(50),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-4).AddHours(16).AddMinutes(6),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-4).AddHours(16).AddMinutes(18),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-4).AddHours(17).AddMinutes(50),
            });              
            
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-3).AddHours(8).AddMinutes(18),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-3).AddHours(10).AddMinutes(52),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-3).AddHours(11).AddMinutes(10),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-3).AddHours(14).AddMinutes(32),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-3).AddHours(14).AddMinutes(47),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-3).AddHours(17),
            });             
            
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-2).AddHours(9).AddMinutes(10),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-2).AddHours(12),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-2).AddHours(12).AddMinutes(13),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-2).AddHours(13).AddMinutes(1),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-2).AddHours(13).AddMinutes(15),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-2).AddHours(15),
            });            
            
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-1).AddHours(8).AddMinutes(45),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(11),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(11).AddMinutes(20),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(13),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(13).AddMinutes(15),
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-1).AddHours(16).AddMinutes(30),
            });            
            
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddHours(14),
                Description = "Przegląd maili, spotkanie z zespołem, praca z projektem"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddHours(16),
                Description = "Pierwsza przerwa"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddHours(16).AddMinutes(15),
                Description = "Powrót do prac nad projektem"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddHours(19),
                Description = "Druga przerwa"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddHours(19).AddMinutes(20),
                Description = "Spotkanie z klientem"
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddHours(16),
                Description = "Koniec pracy"
            });

            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position3User2,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-5).AddHours(9)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-5).AddHours(13)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-5).AddHours(13).AddMinutes(17)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-5).AddHours(17)
            });

            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-2).AddHours(8)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-2).AddHours(12)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-2).AddHours(13).AddMinutes(15)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-2).AddHours(16)
            });

            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-1).AddHours(11)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(12)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(11).AddMinutes(4)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(15)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(15).AddMinutes(10)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-1).AddHours(19)
            });

            for (int i = 1; i <= 7; i++)
            {
                await _context.WorktimeEvents.AddAsync(new()
                {
                    ApplicationUser = _company1Position4User2,
                    EventType = EventType.StartWork,
                    EventTime = DateTime.Today.AddDays(-i).AddHours(8)
                });
                await _context.WorktimeEvents.AddAsync(new()
                {
                    ApplicationUser = _company1Position4User2,
                    EventType = EventType.StartBreak,
                    EventTime = DateTime.Today.AddDays(-i).AddHours(12)
                });
                await _context.WorktimeEvents.AddAsync(new()
                {
                    ApplicationUser = _company1Position4User2,
                    EventType = EventType.EndBreak,
                    EventTime = DateTime.Today.AddDays(-i).AddHours(12).AddMinutes(10)
                });
                await _context.WorktimeEvents.AddAsync(new()
                {
                    ApplicationUser = _company1Position4User2,
                    EventType = EventType.EndWork,
                    EventTime = DateTime.Today.AddDays(-i).AddHours(16)
                });
            }

            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-4).AddHours(10).AddMinutes(10)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-4).AddHours(13)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-4).AddHours(13).AddMinutes(10)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-4).AddHours(15)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-4).AddHours(15).AddMinutes(5)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-4).AddHours(18).AddMinutes(5)
            });

            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-3).AddHours(8).AddMinutes(15)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-3).AddHours(10)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-3).AddHours(10).AddMinutes(5)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-3).AddHours(13)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-3).AddHours(13).AddMinutes(13)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-3).AddHours(16).AddMinutes(10)
            });

            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-2).AddHours(8)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-2).AddHours(12)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-2).AddHours(12).AddMinutes(17)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-2).AddHours(16).AddMinutes(10)
            });

            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartWork,
                EventTime = DateTime.Today.AddDays(-1).AddHours(9)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(11)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(11).AddMinutes(8)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.StartBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(13)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndBreak,
                EventTime = DateTime.Today.AddDays(-1).AddHours(13).AddMinutes(10)
            });
            await _context.WorktimeEvents.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                EventType = EventType.EndWork,
                EventTime = DateTime.Today.AddDays(-1).AddHours(16).AddMinutes(50)
            });



            await _context.SaveChangesAsync();
        }

        private async Task AddVacations()
        {
            if (_context.Vacations.Any())
            {
                return;
            }
            await _context.Vacations.AddAsync(new()
            {
                ApplicationUser = _company1Position3User1,
                IsApproved = true,
                IsRejected = false,
                VacationType = VacationType.AnnualLeave,
                DateFrom = DateTime.Today.AddDays(-14),
                DateTo = DateTime.Today.AddDays(-7),
            });
            await _context.Vacations.AddAsync(new()
            {
                ApplicationUser = _company1Position3User2,
                IsApproved = true,
                IsRejected = false,
                VacationType = VacationType.AnnualLeave,
                DateFrom = DateTime.Today.AddDays(-4),
                DateTo = DateTime.Today.AddDays(4),
            });
            await _context.Vacations.AddAsync(new()
            {
                ApplicationUser = _company1Position3User2,
                IsApproved = true,
                IsRejected = false,
                VacationType = VacationType.AnnualLeave,
                DateFrom = DateTime.Today.AddDays(-3),
                DateTo = DateTime.Today.AddDays(-2),
            });
            await _context.Vacations.AddAsync(new()
            {
                ApplicationUser = _company1Position4User1,
                IsApproved = true,
                IsRejected = false,
                VacationType = VacationType.AnnualLeave,
                DateFrom = DateTime.Today.AddDays(-6),
                DateTo = DateTime.Today.AddDays(-3),
            });
            await _context.Vacations.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                IsApproved = true,
                IsRejected = false,
                VacationType = VacationType.AnnualLeave,
                DateFrom = DateTime.Today.AddDays(-21),
                DateTo = DateTime.Today.AddDays(-15),
            });
            await _context.Vacations.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                IsApproved = true,
                IsRejected = false,
                VacationType = VacationType.SickLeave,
                DateFrom = DateTime.Today.AddDays(-12),
                DateTo = DateTime.Today.AddDays(-9),
            });
            await _context.Vacations.AddAsync(new()
            {
                ApplicationUser = _company1Position4User3,
                IsApproved = true,
                IsRejected = false,
                VacationType = VacationType.AnnualLeave,
                DateFrom = DateTime.Today.AddDays(-6),
                DateTo = DateTime.Today.AddDays(-5),
            });


            await _context.SaveChangesAsync();
        }
    }
}