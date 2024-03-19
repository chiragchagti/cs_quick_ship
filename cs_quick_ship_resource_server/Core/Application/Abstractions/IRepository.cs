using System.Linq.Expressions;

namespace Application.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);
        Task<T> AddAsyncAndGet(T entity);
        Task<bool> AddRangeAsync(ICollection<T> entities);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);
        Task<T> GetAsync(int id);
        Task<bool> RemoveAsync(int id);
        Task<bool> RemoveAsync(T entity);
        Task<bool> SaveAsync();
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateRangeAsync(ICollection<T> entities);
    }
}