using WorkQR.Data.Abstraction;
using WorkQR.Domain;
using WorkQR.EntityFramework;

namespace WorkQR.Data.Repositories
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}