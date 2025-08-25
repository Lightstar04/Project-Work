using InventoryManagement.Domain.Entities;
using InventoryManagement.ViewModels;
using System.Net;

namespace InventoryManagement.Services.Interfaces
{
    public interface IInventoryService
    {        
        Task<InventoryDetailsViewModel> GetByIdAsync(int id);
        
        Task<Inventory> CreateAsync(Inventory inventory);
        
        Task UpdateAsync(int id, Action<Inventory> applyChanges, byte[] version);

        Task DeleteAsync(int id);
        
        Task<int> SearchAsync(string query);

        Task SaveAsync(int id, string title, string description, string customIdTemplateJson, string rowVersionBase64);
        
        Task PostAsync(Post post, int id);
    }
}
