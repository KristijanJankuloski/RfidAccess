using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.Schedule;

namespace RfidAccess.Web.Services.Schedules
{
    public interface IScheduleService
    {
        Task<Result<TimeSlotViewModel>> GetTimeSlots();

        Task<Result> UpdateTimeSlots(TimeSlotViewModel viewModel);
    }
}
