namespace RfidAccess.Web.ViewModels.Schedule
{
    public class TimeSlot
    {
        public string Start { get; set; }

        public string End { get; set; }

        public short Allow { get; set; } = 1;
    }
}
