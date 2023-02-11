using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkQR.Domain;

namespace WorkQR.EntityFramework
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        // Add-Migration -Context ApplicationDbContext -o Migrations <Nazwa migracji>
        // Update-Database -Context ApplicationDbContext
        // Remove-Migration -Context ApplicationDbContext

        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
