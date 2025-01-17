namespace RfidAccess.Web.DataAccess.Repositories.People
{
    using RfidAccess.Web.DataAccess.Repositories.Base;
    using RfidAccess.Web.Models;

    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person?> GetByCode(string code);

        Task<int> CountFilter(string? firstName, string? lastName, string? code);

        Task<List<Person>> GetFiltered(string? firstName, string? lastName, string? code, int skip, int take);
    }
}
