using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.Records;

namespace RfidAccess.Web.Services.Records
{
    public interface IRecordService
    {
        Task<Result> InsertCode(string code);

        Task<Result<RecordsListViewModel>> GetPaginatedRecords(int skip, int take, string? code);
    }
}
