using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetById(string id);
    }
}
