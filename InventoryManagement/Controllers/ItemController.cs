using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Interfaces;
using InventoryManagement.ViewModels.ItemViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
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
            var inventory = await GetInventoryByIdAsync(inventoryId);

            if(ValidateInventory(inventory))
            {
                var viewModel = new CreateViewModel{ Inventory = inventory };
                return View(viewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(int inventoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            await _itemService.UpsertAsync(inventoryId, null, Request.Form, userId);

            return RedirectToAction("Details", "Inventory", new { id = inventoryId });

        }

        [Authorize]
        public async Task<IActionResult> Edit(int inventoryId, int itemId)
        {
            var inventory = await GetInventoryByIdAsync(inventoryId);
            var item = await _itemService.GetAsync(inventoryId, itemId);

            if (ValidateInventory(inventory) && item != null)
            {
                var viewModel = new UpdateViewModel 
                {
                    Inventory = inventory,
                    Item = item
                };
                
                return View(viewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditItem(int inventoryId, int itemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _itemService.UpsertAsync(inventoryId, itemId, Request.Form, userId);

            return RedirectToAction("Details", "Inventory", new { id = inventoryId });
        }

        [Authorize]
        public async Task<IActionResult> Action(string[] selected, string operation, int inventoryId)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (ValidateInventory(inventory))
            {
                await _itemService.ButtonOperationAsync(selected, operation);
                
                return RedirectToAction("Details", "Inventory", new {id = inventoryId});
            }

            return NotFound();
        }

        private bool ValidateInventory(Inventory inventory)
        {
            if (inventory != null && inventory.IsPublic == true
                || User.IsInRole("Admin") || inventory.OwnerId == User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            {
                return true;
            }

            return false;
        }

        private async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            var entity = await _context.Inventories
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.Id == inventoryId);
            
            return entity;
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditSelected(string[] selected, int inventoryId)
        {
            if(selected.Length != 1)
            {
                TempData["Error"] = "Please, select exactly one item to edit";
                return RedirectToAction("Details", "Inventory", new { id = inventoryId });
            }

            var itemId = int.Parse(selected[0]);
            return RedirectToAction("Edit", "Item", new {inventoryId, itemId});
        }

    }
}
