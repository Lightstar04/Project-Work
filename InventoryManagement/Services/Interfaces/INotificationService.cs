namespace InventoryManagement.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyPostAsync(int inventoryId, object payload);
    }
}
