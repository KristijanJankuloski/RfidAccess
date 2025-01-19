using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RfidAccess.Web.Services.Schedules;
using RfidAccess.Web.ViewModels.Schedule;

namespace RfidAccess.Web.Controllers
{
    [Authorize]
    public class ScheduleController(IScheduleService scheduleService) : Controller
    {
        private readonly IScheduleService scheduleService = scheduleService;

        public async Task<IActionResult> Index()
        {
            var result = await scheduleService.GetTimeSlots();
            if (result.IsFailed)
            {
                return View(new TimeSlotViewModel());
            }
            return View(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TimeSlotViewModel timeSlotsVm)
        {
            timeSlotsVm.Monday = timeSlotsVm.Monday.Where(x => x.Start != null && x.End != null).ToList();
            timeSlotsVm.Tuesday = timeSlotsVm.Tuesday.Where(x => x.Start != null && x.End != null).ToList();
            timeSlotsVm.Wednesday = timeSlotsVm.Wednesday.Where(x => x.Start != null && x.End != null).ToList();
            timeSlotsVm.Thursday = timeSlotsVm.Thursday.Where(x => x.Start != null && x.End != null).ToList();
            timeSlotsVm.Friday = timeSlotsVm.Friday.Where(x => x.Start != null && x.End != null).ToList();
            timeSlotsVm.Saturday = timeSlotsVm.Saturday.Where(x => x.Start != null && x.End != null).ToList();
            timeSlotsVm.Sunday = timeSlotsVm.Sunday.Where(x => x.Start != null && x.End != null).ToList();

            try
            {
                var result = await scheduleService.UpdateTimeSlots(timeSlotsVm);
                if (result.IsFailed)
                {
                    TempData["Error"] = result.Message;
                }
                TempData["Success"] = "Распоред ажуриран";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
