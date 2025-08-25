using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? Theme { get; set; } = "light";
        public bool IsBlocked { get; set; } = false;
        public virtual ICollection<Inventory> OwnerOfInventories { get; set; } = new List<Inventory>();
        public virtual ICollection<InventoryAccess> InventoryAccesses { get; set; } = new List<InventoryAccess>();
    }
}
