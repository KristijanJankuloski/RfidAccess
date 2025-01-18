using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RfidAccess.Web.Services.ErrorLogs;

namespace RfidAccess.Web.Controllers
{
    [Authorize]
    public class ErrorLogsController(IErrorLogService errorLogService) : Controller
    {
        private readonly IErrorLogService errorLogService = errorLogService;

        public async Task<IActionResult> Index(
            [FromQuery] int? page)
        {
            try
            {
                page ??= 1;
                int take = 10;
                int skip = ((int)page - 1) * take;
                if (skip < 0)
                {
                    TempData["Error"] = "Недозволено пребарување";
                    return RedirectToAction("Index");
                }
                var result = await errorLogService.GetLogs(skip, take);
                if (result.IsFailed)
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Index", "Home");
                }

                return View(result.Value);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
