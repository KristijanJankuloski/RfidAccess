namespace RfidAccess.Web.ViewModels.ErrorLogs
{
    public class ErrorLogsListViewModel
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public int Total { get; set; } = 0;

        public List<ErrorLogViewModel> ErrorLogs { get; set; } = [];
    }
}
