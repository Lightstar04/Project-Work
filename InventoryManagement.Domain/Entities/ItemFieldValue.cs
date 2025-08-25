namespace InventoryManagement.Domain.Entities
{
    public class ItemFieldValue
    {
        public int Id { get; set; }
       
        public int ItemId { get; set; }
        public virtual Item? Item { get; set; }
        
        public int InventoryFieldId { get; set; }
        public virtual InventoryField? InventoryField { get; set; }
        
        public string? Value { get; set; }
    }
}
