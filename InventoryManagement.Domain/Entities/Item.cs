namespace InventoryManagement.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        
        public int InventoryId { get; set; }
        public virtual Inventory? Inventory { get; set; }
        
        public string CustomId { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public string OwnerId { get; set; } = "";
        public virtual AppUser? CreatedBy { get; set; }
        
        public List<ItemFieldValue> FieldValues { get; set; } = new();
        public byte[]? Version { get; set; }
    }
}
