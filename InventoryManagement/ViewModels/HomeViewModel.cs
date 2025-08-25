using InventoryManagement.Domain.Entities;

namespace InventoryManagement.ViewModels
{
    public class HomeViewModel
    {
        public List<Inventory> Inventories { get; set; } = new();
        public List<Inventory> Top {  get; set; } = new();
    }
}
