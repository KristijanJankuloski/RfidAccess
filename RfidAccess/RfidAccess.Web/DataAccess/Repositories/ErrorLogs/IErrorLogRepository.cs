using RfidAccess.Web.DataAccess.Repositories.Base;
using RfidAccess.Web.Models;

namespace RfidAccess.Web.DataAccess.Repositories.ErrorLogs
{
    public interface IErrorLogRepository : IRepository<ErrorLog>
    {
        Task<List<ErrorLog>> GetFromDates(DateTime start, DateTime end);
    }
}
