using Microsoft.AspNetCore.SignalR;

namespace InventoryManagement.Hubs
{
    public class InventoryHub : Hub
    {
        public void JoinInventory(string inventoryId)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, $"inventory - {inventoryId}");
        }

        public void LeaveInventory(string inventoryId)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, $"inventory - {inventoryId}");
        }
    }
}
