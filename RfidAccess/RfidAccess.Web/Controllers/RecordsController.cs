using Microsoft.AspNetCore.Mvc;

namespace RfidAccess.Web.Controllers
{
    public class RecordsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
