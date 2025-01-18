using RfidAccess.Web.DataAccess.Repositories.ErrorLogs;
using RfidAccess.Web.Models;
using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.ErrorLogs;

namespace RfidAccess.Web.Services.ErrorLogs
{
    public class ErrorLogService(IErrorLogRepository errorLogRepository) : IErrorLogService
    {
        private readonly IErrorLogRepository errorLogRepository = errorLogRepository;

        public async Task<Result<ErrorLogsListViewModel>> GetLogs(int skip, int take)
        {
            int count = await errorLogRepository.Count();
            List<ErrorLog> errorLogs = await errorLogRepository.Filter(q => q
            .OrderByDescending(x => x.Id)
            .Skip(skip)
            .Take(take));

            return new Result<ErrorLogsListViewModel>(new ErrorLogsListViewModel
            {
                Skip = skip,
                Take = take,
                Total = count,
                ErrorLogs = errorLogs.Select(x => new ErrorLogViewModel
                {
                    Id = x.Id,
                    Message = x.Message,
                    Code = x.Code,
                    CreatedOn = x.CreatedOn
                }).ToList()
            });
        }
    }
}
