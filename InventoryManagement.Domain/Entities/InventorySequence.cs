namespace InventoryManagement.Domain.Entities
{
    public class InventorySequence
    {
        public int Id {  get; set; }
        public int InventoryId {  get; set; }
        public long NextValue {  get; set; }
    }
}
