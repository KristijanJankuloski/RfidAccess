namespace RfidAccess.Web.Models
{
    public class ErrorLog : BaseEntity
    {
        public string Code { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public int? PersonId { get; set; }

        public Person? Person { get; set; }
    }
}
