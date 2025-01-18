namespace RfidAccess.Web.ViewModels.ErrorLogs
{
    public class ErrorLogViewModel
    {
        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }
}
