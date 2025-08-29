using InventoryManagement.Domain.Entities;

namespace InventoryManagement.ViewModels.InventoryViewModels
{
    public class DetailsViewModel
    {
        public Inventory Inventory { get; set; } = new();
        public List<Item> Items { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
    }
}
