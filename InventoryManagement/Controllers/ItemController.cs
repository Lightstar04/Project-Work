using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Interfaces;
using InventoryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly InventoryManagementDbContext _context;
        private readonly IItemService _itemService;

        public ItemController(InventoryManagementDbContext context, IItemService itemService)
        {
            _context = context;
            _itemService = itemService;
        }

        [Authorize]
        public async Task<IActionResult> Create(int inventoryId)
        {
            var inventory = _context.Inventories.Include(i => i.Fields).FirstOrDefault(i => i.Id ==  inventoryId);

            if(inventory != null && inventory.IsPublic == true || User.IsInRole("Admin"))
            {
                var viewModel = new CreateItemViewModel{ Inventory = inventory };
                return View(viewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(int inventoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var inventory = await _context.Inventories
                .Include(i => i.Fields)
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (inventory != null)
            {
                var values = new List<ItemFieldValue>();

                foreach(var field in inventory.Fields)
                {
                    var key = $"field_{field.Id}";
                    
                    var value = Request.Form[key];

                    values.Add(new ItemFieldValue { InventoryFieldId = field.Id, Value = value });
                }

                var item = await _itemService.CreateAsync(inventoryId, values, userId);
                
                return RedirectToAction("Details", "Inventory", new { id = inventoryId });
            }

            return NotFound();

        }

        [Authorize]
        public async Task<IActionResult> Action(string[] selected, string operation)
        {
            await _itemService.ButtonOperationAsync(selected, operation);

            return RedirectToAction("Details", "Inventory");
        }

    }
}
