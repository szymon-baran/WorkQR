using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Models;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Infrastructure.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ApplicationUser>> GetCompanyEmployeesListByCompanyId(Guid companyId)
        {
            return await GetWithConditionAsync(x => (!x.LockoutEnd.HasValue
                || x.LockoutEnd.Value <= DateTime.Now)
                && x.Position != null
                && x.Position.CompanyId == companyId
                && x.Position.UserRoleName != "QRScanner");
        }
    }
}