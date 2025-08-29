using InventoryManagement.Domain.Entities;

namespace InventoryManagement.ViewModels.ItemViewModels
{
    public class UpdateViewModel
    {
        public Inventory Inventory { get; set; } = null;
        public Item Item { get; set; } = null;
    }
}
