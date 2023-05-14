using WorkQR.Data.Abstraction;
using WorkQR.Domain;
using WorkQR.EntityFramework;

namespace WorkQR.Data.Repositories
{
    public class VacationRepository : Repository<Vacation>, IVacationRepository
    {
        public VacationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}