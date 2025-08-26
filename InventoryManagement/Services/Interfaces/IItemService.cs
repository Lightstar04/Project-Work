using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Services.Interfaces
{
    public interface IItemService
    {
        Task<Item> CreateAsync(int inventoryId, List<ItemFieldValue> values, string userId);
        
        Task<Item?> GetAsync(int inventoryId, int id);
        
        Task UpdateAsync(int id);
        
        Task RemoveAsync(int id);
        
        Task ButtonOperationAsync(string[] selected, string operation);
    }
}
