using InventoryManagement.Hubs;
using InventoryManagement.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace InventoryManagement.Services.Classes
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<InventoryHub> _hub;

        public NotificationService(IHubContext<InventoryHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyPostAsync(int inventoryId, object payload) =>
            await _hub.Clients.Group($"inventory - {inventoryId}").SendAsync("ReceivePost", payload);
    }
}
