using RfidAccess.Web.ViewModels.Base;

namespace RfidAccess.Web.Services.Records
{
    public interface IRecordService
    {
        Task<Result> InsertCode(string code);
    }
}
