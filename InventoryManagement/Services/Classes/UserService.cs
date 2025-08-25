using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly InventoryManagementDbContext _context;

        public UserService(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetById(string id)
        {
            var user = await _context.Users
                .Include(u => u.OwnerOfInventories)
                .Include(u => u.InventoryAccesses)
                .ThenInclude(a => a.Inventory)
                .FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }
    }
}
