using Microsoft.EntityFrameworkCore;
using RfidAccess.Web.DataAccess.Context;
using RfidAccess.Web.DataAccess.Repositories.Base;
using RfidAccess.Web.Models;

namespace RfidAccess.Web.DataAccess.Repositories.Records
{
    public sealed class RecordRepository(RfidContext context)
        : BaseRepository<Record>(context), IRecordRepository
    {
        public async Task<List<Record>> GetFromDates(DateTime start, DateTime end)
        {
            return await context.Records
                .Include(x => x.Person)
                .Where(x => x.Time >= start && x.Time <= end)
                .ToListAsync();
        }

        public override async Task<List<Record>> GetRange(int skip, int take)
        {
            return await context.Records
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .Include(x => x.Person)
                .ToListAsync();
        }
    }
}
