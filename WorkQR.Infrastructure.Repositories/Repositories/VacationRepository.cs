using WorkQR.Infrastructure.Abstraction;
using WorkQR.Domain.Models;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Infrastructure.Repositories
{
    public class VacationRepository : Repository<Vacation>, IVacationRepository
    {
        public VacationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}