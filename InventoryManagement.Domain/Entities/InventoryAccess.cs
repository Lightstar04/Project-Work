namespace InventoryManagement.Domain.Entities
{
    public class InventoryAccess
    {
        public int Id { get; set; }
        
        public int InventoryId { get; set; }
        public Inventory? Inventory { get; set; }
        
        public string UserId { get; set; } = "";
        public virtual AppUser? User { get; set; }
    }
}
