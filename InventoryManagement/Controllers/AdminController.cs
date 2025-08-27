using InventoryManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var usersList = await _service.GetAsync();

            return View(usersList);
        }

        public async Task<IActionResult> Action(string[] selected, string operation)
        {
            await _service.ButtonOperationAsync(selected, operation);

            return RedirectToAction("Index");
        }
    }
}
