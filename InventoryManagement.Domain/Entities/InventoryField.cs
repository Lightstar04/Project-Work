using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Entities
{
    public class InventoryField
    {
        public int Id { get; set; }
        
        public int InventoryId { get; set; }
        public virtual Inventory? Inventory { get; set; }
        
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public InventoryFieldType FieldType { get; set; }
        public bool ShowInTable { get; set; } = false;
        public int Order { get; set; } = 0;
    }
}
