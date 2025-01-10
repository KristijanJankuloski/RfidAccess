namespace RfidAccess.Web.DataAccess.Repositories.Base
{
    using RfidAccess.Web.Models;

    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAll();

        Task<int> Count();

        Task<List<T>> GetRange(int skip, int take);

        Task<T?> GetById(int id);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task Delete(int id);

        Task SaveChanges();

        Task<List<T>> Filter(Func<IQueryable<T>, IQueryable<T>> func);
    }
}
