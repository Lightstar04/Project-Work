using InventoryManagement.Domain.Entities;
using InventoryManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _service.ValidateUserAsync(userId);

            if (result)
            {
                var usersList = await _service.GetAsync();

                return View(usersList);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Action(string[] selected, string operation)
        {
            await _service.ButtonOperationAsync(selected, operation);

            return RedirectToAction("Index");
        }
    }
}
