namespace RfidAccess.Web.Models
{
    public class WeekTimeSlots : BaseEntity
    {
        public string Monday { get; set; }

        public string Tuesday { get; set; }

        public string Wednesday { get; set; }

        public string Thursday { get; set; }

        public string Friday { get; set; }

        public string Saturday { get; set; }

        public string Sunday { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
