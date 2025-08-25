using InventoryManagement.Domain.Entities;

namespace InventoryManagement.ViewModels
{
    public class InventoryDetailsViewModel
    {
        public Inventory Inventory { get; set; } = null;
        public List<Item> Items { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
    }
}
