using RfidAccess.Web.Helpers;
using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.Schedule;

namespace RfidAccess.Web.Services.Schedules
{
    public interface IScheduleService
    {
        Task<Result<TimeSlotViewModel>> GetTimeSlots();

        Task<Result<ConvertedTimeSlot>> GetActiveTimeSlot();

        Task<Result<ActiveTimeSlotViewModel>> GetActiveAndNext();

        Task<Result> UpdateTimeSlots(TimeSlotViewModel viewModel);
    }
}
