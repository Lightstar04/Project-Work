using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Entities
{
    public class CustomIdElement
    {
        public CustomIdElementType Type {  get; set; }
        public string? Parameter {  get; set; }
    }
}
