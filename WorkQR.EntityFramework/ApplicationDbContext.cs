using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkQR.Domain;

namespace WorkQR.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Add-Migration -Context ApplicationDbContext -o Migrations <Nazwa migracji>
        // Update-Database -Context ApplicationDbContext
        // Remove-Migration -Context ApplicationDbContext

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<WorktimeEvent> WorktimeEvents { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
