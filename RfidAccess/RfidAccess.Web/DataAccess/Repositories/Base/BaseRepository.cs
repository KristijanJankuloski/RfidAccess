using Microsoft.EntityFrameworkCore;
using RfidAccess.Web.DataAccess.Context;
using RfidAccess.Web.Models;

namespace RfidAccess.Web.DataAccess.Repositories.Base
{
    public abstract class BaseRepository<T>(RfidContext context) : IRepository<T> where T : BaseEntity
    {
        private readonly RfidContext context = context;

        public async Task<int> Count()
        {
            return await context.Set<T>().CountAsync();
        }

        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public async Task Delete(int id)
        {
            T? entity = await GetById(id);
            if (entity == null)
            {
                return;
            }

            context.Set<T>().Remove(entity);
        }

        public async Task<List<T>> Filter(Func<IQueryable<T>, IQueryable<T>> func)
        {
            return await func(context.Set<T>()).ToListAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<T>> GetRange(int skip, int take)
        {
            return await context.Set<T>()
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
