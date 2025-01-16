using RfidAccess.Web.Helpers;

namespace RfidAccess.Web.ViewModels.Schedule
{
    public class ActiveTimeSlotViewModel
    {
        public ConvertedTimeSlot? Active { get; set; }

        public ConvertedTimeSlot? Next { get; set; }
    }
}
