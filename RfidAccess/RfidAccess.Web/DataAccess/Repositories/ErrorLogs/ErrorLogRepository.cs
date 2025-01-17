using Microsoft.EntityFrameworkCore;
using RfidAccess.Web.DataAccess.Context;
using RfidAccess.Web.DataAccess.Repositories.Base;
using RfidAccess.Web.Models;

namespace RfidAccess.Web.DataAccess.Repositories.ErrorLogs
{
    public class ErrorLogRepository(RfidContext context)
        : BaseRepository<ErrorLog>(context), IErrorLogRepository
    {
        public async Task<List<ErrorLog>> GetFromDates(DateTime start, DateTime end)
        {
            return await context.ErrorLogs
                .Include(x => x.Person)
                .Where(x => x.CreatedOn >= start && x.CreatedOn < end)
                .ToListAsync();
        }
    }
}
