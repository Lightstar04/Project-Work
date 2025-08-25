using Azure;

namespace InventoryManagement.Domain.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Category { get; set; } = "Other";
        public bool IsPublic { get; set; } = false;
        public string? CustomIdTemplateJson { get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
        
        public string OwnerId { get; set; } = "";
        public virtual AppUser? Owner { get; set; }
        
        public List<InventoryField> Fields { get; set; } = new();
        public List<InventoryAccess> AccessList { get; set; } = new();
        public List<Item> Items { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
        public byte[]? Version { get; set; }
    }
}
