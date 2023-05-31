using WorkQR.Domain.Models;

namespace WorkQR.Infrastructure.Abstraction
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<IEnumerable<ApplicationUser>> GetCompanyEmployeesListByCompanyId(Guid companyId);
    }
}