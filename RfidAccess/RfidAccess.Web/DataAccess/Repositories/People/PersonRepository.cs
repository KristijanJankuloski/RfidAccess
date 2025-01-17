namespace RfidAccess.Web.DataAccess.Repositories.People
{
    using Microsoft.EntityFrameworkCore;
    using RfidAccess.Web.DataAccess.Context;
    using RfidAccess.Web.DataAccess.Repositories.Base;
    using RfidAccess.Web.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class PersonRepository(RfidContext context)
        : BaseRepository<Person>(context), IPersonRepository
    {
        public async Task<int> CountFilter(string? firstName, string? lastName, string? code)
        {
            IQueryable<Person> query = context.People;

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                string firstNameNormal = firstName.Trim();
                query = query.Where(x => x.FirstName.Contains(firstNameNormal));
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                string lastNameNormal = lastName.Trim();
                query = query.Where(x => x.LastName.Contains(lastNameNormal));
            }
            if (!string.IsNullOrWhiteSpace(code))
            {
                code = code.Trim();
                query = query.Where(x => x.Code == code);
            }

            return await query.CountAsync();
        }

        public async Task<Person?> GetByCode(string code)
        {
            return await context.People.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Person>> GetFiltered(string? firstName, string? lastName, string? code, int skip, int take)
        {
            IQueryable<Person> query = context.People;

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                string firstNameNormal = firstName.Trim();
                query = query.Where(x => x.FirstName.Contains(firstNameNormal));
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                string lastNameNormal = lastName.Trim();
                query = query.Where(x => x.LastName.Contains(lastNameNormal));
            }
            if (!string.IsNullOrWhiteSpace(code))
            {
                code = code.Trim();
                query = query.Where(x => x.Code == code);
            }

            return await query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
