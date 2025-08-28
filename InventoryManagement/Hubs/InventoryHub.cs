using Microsoft.AspNetCore.SignalR;

namespace InventoryManagement.Hubs
{
    public class InventoryHub : Hub
    {
        public async Task JoinInventory(string inventoryId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"inventory - {inventoryId}");
        }

        public async Task LeaveInventory(string inventoryId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"inventory - {inventoryId}");
        }
    }
}
