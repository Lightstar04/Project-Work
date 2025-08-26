using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryManagement.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _service;
        private readonly InventoryManagementDbContext _context;

        public InventoryController(IInventoryService service, InventoryManagementDbContext context)
        {
            _service = service;
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return View(result);
        }

        [Authorize]
        public IActionResult Create()
        {
            var inventory = new Inventory
            {
                Fields = new List<InventoryField>
                {
                    new InventoryField(),
                    new InventoryField()
                },
                Tags = new List<Tag>
                {
                    new Tag()
                }
            };

            return View(inventory);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            inventory.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            
            var result = await _service.CreateAsync(inventory);

            return RedirectToAction("Details", new { id = result.Id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveSettings(
            int id, 
            string title,
            string description, 
            string rowVersionBase64) 
        {
            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null || (inventory.OwnerId != userId && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            var result = _service.SaveAsync(id, title, description, rowVersionBase64);

            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> SaveSettings(int id, string customIdTemplateJson, string rowVersionBase64)
        {
            var result = _service.SaveAsync(id, customIdTemplateJson, rowVersionBase64);

            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> PostComment(int id, string message)
        {
            if(!User.Identity?.IsAuthenticated ?? true)
            {
                return Forbid();
            }

            var post = new Post
            {
                InventoryId = id,
                AuthorId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Markdown = message
            };

            await _service.PostAsync(post, id);

            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction("Details");
        }
    }
}
