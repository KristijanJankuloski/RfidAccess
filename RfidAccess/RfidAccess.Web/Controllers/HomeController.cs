using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RfidAccess.Web.Services.Schedules;
using RfidAccess.Web.ViewModels;
using System.Diagnostics;

namespace RfidAccess.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScheduleService scheduleService;

        public HomeController(ILogger<HomeController> logger, IScheduleService scheduleService)
        {
            _logger = logger;
            this.scheduleService = scheduleService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await scheduleService.GetActiveAndNext();
                if (result.IsFailed)
                {
                    return View(null);
                }
                return View(result.Value);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
