using RfidAccess.Web.ViewModels.Base;

namespace RfidAccess.Web.Services.Export
{
    public interface IExportService
    {
        Task<Result<byte[]>> ExportRecords(DateTime startDate, DateTime endDate);
    }
}
