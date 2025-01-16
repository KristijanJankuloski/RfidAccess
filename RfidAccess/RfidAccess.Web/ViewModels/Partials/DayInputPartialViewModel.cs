using RfidAccess.Web.ViewModels.Schedule;

namespace RfidAccess.Web.ViewModels.Partials
{
    public class DayInputPartialViewModel
    {
        public DayInputPartialViewModel(List<TimeSlot> daySlots, string dayName, string title)
        {
            DaySlots = daySlots;
            DayName = dayName;
            Title = title;
        }

        public List<TimeSlot> DaySlots { get; set; } = [];

        public string DayName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
    }
}
