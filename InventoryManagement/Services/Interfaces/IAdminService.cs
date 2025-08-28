using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<AppUser>> GetAsync();

        Task ButtonOperationAsync(string[] selected, string operation);

        Task<bool> ValidateUserAsync(string userId);
    }
}
