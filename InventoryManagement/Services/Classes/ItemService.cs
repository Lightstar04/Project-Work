using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services.Classes
{
    public class ItemService : IItemService
    {
        private readonly InventoryManagementDbContext _context;
        private readonly ICustomIdService _customId;

        public ItemService(InventoryManagementDbContext context, ICustomIdService customId)
        {
            _context = context; 
            _customId = customId;
        }

        public async Task<Item?> GetAsync(int inventoryId, int id)
        {
            var result = await _context.Items
                .Include(i => i.FieldValues)
                .FirstOrDefaultAsync(i => i.Id == id && i.InventoryId == inventoryId);

            return result;
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);

            if(entity != null)
            {
                _context.Items.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ButtonOperationAsync(string[] selected, string operation)
        {
            var ids = selected.ToList();
            var items = await _context.Items.Where(i => ids.Contains(i.Id.ToString())).ToListAsync();

            foreach (var item in items)
            {
                if (operation == "delete")
                    await RemoveAsync(item.Id);
            }
        }

        public async Task<Item> UpsertAsync(
            int inventoryId, int? itemId,
            IFormCollection form, string userId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Fields)
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (inventory is null)
                throw new KeyNotFoundException("Inventory not found.");

            Item? item = null;

            if (itemId.HasValue)
            {
                item = await _context.Items
                    .Include(i => i.FieldValues)
                    .FirstOrDefaultAsync(i => i.Id == itemId.Value && i.InventoryId == inventoryId);

                if (item is null)
                    throw new KeyNotFoundException("Item not found.");
            }
            else
            {
                var customId = await _customId.GenerateForInventoryAsync(inventoryId);
                
                item = new Item
                {
                    InventoryId = inventoryId,
                    CustomId = customId,
                    OwnerId = userId
                };

                _context.Items.Add(item);
                
                await _context.SaveChangesAsync();
            }

            foreach (var field in inventory.Fields)
            {
                var key = $"field_{field.Id}";
                var value = form[key];

                var existing = item.FieldValues?.Find(f => f.InventoryFieldId == field.Id);

                if (existing != null)
                {
                    existing.Value = value;
                    _context.ItemFieldValues.Update(existing);
                }
                else
                {
                    var fieldVal = new ItemFieldValue
                    {
                        InventoryFieldId = field.Id,
                        ItemId = item.Id,
                        Value = value
                    };
                    
                    _context.ItemFieldValues.Add(fieldVal);
                }
            }

            await _context.SaveChangesAsync();
            return item;
        }
    }
}
