using Microsoft.AspNetCore.Mvc;
using RfidAccess.Web.Services.People;
using RfidAccess.Web.ViewModels.People;

namespace RfidAccess.Web.Controllers
{
    public class PeopleController(IPersonService personService) : Controller
    {
        private readonly IPersonService personService = personService;

        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await personService.GetAllPeople();
                if (result.IsFailed)
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Index", "Home");
                }

                return View(result.Value);
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonCreateViewModel person)
        {
            try
            {
                var result = personService.CreatePerson(person);
                if (result.IsFailed)
                {
                    TempData["Error"] = result.Message;
                }
                else
                {
                    TempData["Success"] = "Created";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
