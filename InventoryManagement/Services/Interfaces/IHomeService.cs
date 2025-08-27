using InventoryManagement.ViewModels;

namespace InventoryManagement.Services.Interfaces
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetAsync(string query);
    }
}
