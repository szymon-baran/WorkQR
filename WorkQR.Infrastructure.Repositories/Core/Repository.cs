using IdentityModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WorkQR.Infrastructure.Abstraction;
using WorkQR.Infrastructure.EntityFramework;

namespace WorkQR.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void UpdateRange(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public T? Find(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await _context.Set<T>().Where(whereCondition).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetWithConditionAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await _context.Set<T>().Where(whereCondition).ToListAsync();
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public Task<bool> AnyAsync(Expression<Func<T, bool>> whereCondition)
        {
            return _context.Set<T>().AnyAsync(whereCondition);
        }
    }
}