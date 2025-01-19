using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RfidAccess.Web.Helpers;
using RfidAccess.Web.Services.Schedules;
using RfidAccess.Web.ViewModels;
using System.Diagnostics;

namespace RfidAccess.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScheduleService scheduleService;
        private readonly IHostEnvironment hostEnvironment;

        public HomeController(ILogger<HomeController> logger, IScheduleService scheduleService, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            this.scheduleService = scheduleService;
            this.hostEnvironment = hostEnvironment;
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

        [Authorize]
        public async Task<IActionResult> Backup()
        {
            try
            {
                string databasePath = Path.Combine(hostEnvironment.ContentRootPath, "default.db");
                string backupName = "backup.sqlite";
                var result = await BackupHelper.BackupDatabase(databasePath, backupName);
                if (result.IsFailed)
                {
                    TempData["Error"] = result.Message;
                }
                else
                {
                    TempData["Success"] = "Направен backup";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
