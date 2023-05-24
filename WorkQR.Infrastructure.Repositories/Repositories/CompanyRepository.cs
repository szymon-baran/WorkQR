using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Models;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Infrastructure.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}