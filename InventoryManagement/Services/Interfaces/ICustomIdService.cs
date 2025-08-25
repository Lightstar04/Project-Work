namespace InventoryManagement.Services.Interfaces
{
    public interface ICustomIdService
    {
        Task<string> GenerateForInventoryAsync(int inventoryId, CancellationToken token = default);
        string RenderPreview(string jsonTemplate);
        bool ValidateFormat(string jsonTemplate, string candidate);
    }
}
