

using Application.Abstractions;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbset = _context.Set<T>();
        }

        public async Task<bool> AddAsync(T entity)
        {
            dbset.Add(entity);
            return await SaveAsync();
        }
        public async Task<T> AddAsyncAndGet(T entity)
        {
            dbset.Add(entity);
            var saveResult = await SaveAsync(); // Save the changes to the database

            if (saveResult)
            {
                return entity; // Return the entity after it has been added and saved
            }
            return null;
        }

        public async Task<bool> AddRangeAsync(ICollection<T> entities)
        {
            dbset.AddRange(entities);
            return await SaveAsync();
        }

        public async Task<bool> UpdateRangeAsync(ICollection<T> entities)
        {
            dbset.UpdateRange(entities);
            return await SaveAsync();
        }


        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            // You should use .FirstOrDefaultAsync() for asynchronous querying.
            // Ensure that the query is awaited and the result is returned.
            return await query.FirstOrDefaultAsync();
        }


        public async Task<T> GetAsync(int id)
        {
            return await dbset.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
                query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderBy != null)
                return await orderBy(query).ToListAsync(); // async-await added here
            return await query.ToListAsync(); // async-await added here
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            dbset.Remove(entity);
            return await SaveAsync();

        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                dbset.Remove(entity);
                return await SaveAsync();
            }
            return false;
        }

        public async Task<bool> SaveAsync()
        {
            int result = await _context.SaveChangesAsync();
            return result == 1 ? true : false;
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            _context.ChangeTracker.Clear();
            dbset.Update(entity);
            return await SaveAsync();
        }


    }
}
