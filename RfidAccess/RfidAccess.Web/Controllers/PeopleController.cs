using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RfidAccess.Web.Models;
using RfidAccess.Web.Services.People;
using RfidAccess.Web.ViewModels.People;

namespace RfidAccess.Web.Controllers
{
    [Authorize]
    public class PeopleController(IPersonService personService) : Controller
    {
        private readonly IPersonService personService = personService;

        public async Task<IActionResult> Index(
            [FromQuery] int? page = 1,
            [FromQuery] string? firstName = null,
            [FromQuery] string? lastName = null,
            [FromQuery] string? code = null)
        {
            try
            {
                int take = 10;
                page ??= 1;
                int skip = ((int)page - 1) * take;
                if (skip < 0)
                {
                    TempData["Error"] = "Недозволено пребарување";
                    return RedirectToAction("Index");
                }
                var result = await personService.GetPaginated(skip, take, firstName, lastName, code);
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await personService.DeletePerson(id);
                if (result.IsFailed)
                {
                    TempData["Error"] = result.Message;
                }
                else
                {
                    TempData["Success"] = "Deleted";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await personService.ToggleWhiteListPerson(id);
                if (result.IsFailed)
                {
                    TempData["Error"] = result.Message;
                }
                else
                {
                    TempData["Success"] = "Updated";
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
