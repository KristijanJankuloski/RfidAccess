namespace RfidAccess.Web.DataAccess.Repositories.Records
{
    using RfidAccess.Web.DataAccess.Repositories.Base;
    using RfidAccess.Web.Models;

    public interface IRecordRepository : IRepository<Record>
    {
        Task<List<Record>> GetFromDates(DateTime start, DateTime end);
    }
}
