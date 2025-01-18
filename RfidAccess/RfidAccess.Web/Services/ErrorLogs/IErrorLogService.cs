using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.ErrorLogs;

namespace RfidAccess.Web.Services.ErrorLogs
{
    public interface IErrorLogService
    {
        Task<Result<ErrorLogsListViewModel>> GetLogs(int skip, int take);
    }
}
