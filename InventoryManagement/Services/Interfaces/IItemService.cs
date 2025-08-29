using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Services.Interfaces
{
    public interface IItemService
    {        
        Task<Item?> GetAsync(int inventoryId, int id);
        
        Task RemoveAsync(int id);
        
        Task ButtonOperationAsync(string[] selected, string operation);

        Task<Item> UpsertAsync(int inventoryId, int? itemId, IFormCollection collection, string userId);
    }
}
