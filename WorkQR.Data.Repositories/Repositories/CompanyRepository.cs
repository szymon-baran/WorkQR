using WorkQR.Data.Abstraction;
using WorkQR.Domain;
using WorkQR.EntityFramework;

namespace WorkQR.Data.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}