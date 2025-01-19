namespace RfidAccess.Web.ViewModels.Schedule
{
    public class TimeSlotViewModel
    {
        public List<TimeSlot> Monday { get; set; } = new List<TimeSlot>();
        public List<TimeSlot> Tuesday { get; set; } = new List<TimeSlot>();
        public List<TimeSlot> Wednesday { get; set; } = new List<TimeSlot>();
        public List<TimeSlot> Thursday { get; set; } = new List<TimeSlot>();
        public List<TimeSlot> Friday { get; set; } = new List<TimeSlot>();
        public List<TimeSlot> Saturday { get; set; } = new List<TimeSlot>();
        public List<TimeSlot> Sunday { get; set; } = new List<TimeSlot>();
        public DateTime? LastModified { get; set; }
    }
}
