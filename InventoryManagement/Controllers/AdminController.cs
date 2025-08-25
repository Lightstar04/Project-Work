using InventoryManagement.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly AdminStore _store;

        public AdminController(AdminStore store)
        {
            _store = store;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var usersList = await _store.Get();

            return View(usersList);
        }

        public async Task<IActionResult> Action(string[] selected, string operation)
        {
            await _store.ButtonOperation(selected, operation);

            return RedirectToAction("Index");
        }
    }
}
