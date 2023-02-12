using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WorkQR.Data.Abstraction;
using WorkQR.EntityFramework;

namespace WorkQR.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
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
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}