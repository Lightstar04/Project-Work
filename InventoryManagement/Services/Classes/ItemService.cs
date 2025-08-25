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

        public async Task<Item> CreateAsync(int inventoryId, List<ItemFieldValue> values, string userId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Fields)
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (inventory == null)
            {
                throw new KeyNotFoundException();
            }

            var customId = await _customId.GenerateForInventoryAsync(inventoryId);
            
            var item = new Item
            {
                InventoryId = inventoryId,
                CustomId = customId,
                OwnerId = userId
            };
           
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            foreach (var v in values)
            {
                v.ItemId = item.Id;
                _context.ItemFieldValues.Add(v);
            }
            
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item?> GetAsync(int inventoryId, int id)
        {
            var result = await _context.Items
                .Include(i => i.FieldValues)
                .FirstOrDefaultAsync(i => i.Id == id && i.InventoryId == inventoryId);

            return result;
        }

        public async Task UpdateAsync(Item item, byte[]? version)
        {
            if (version != null) 
            {
                item.Version = version;
            }
            
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int inventoryId, int id)
        {
            var entity = await GetAsync(inventoryId, id);

            if(entity != null)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
