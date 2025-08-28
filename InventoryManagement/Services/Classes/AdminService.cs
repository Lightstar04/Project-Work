using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services.Classes
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly InventoryManagementDbContext _context;

        public AdminService(UserManager<AppUser> userManager, InventoryManagementDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<AppUser>> GetAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }

        public async Task ButtonOperationAsync(string[] selected, string operation)
        {
            var ids = selected.ToList();
            var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

            int count = 0;

            foreach (var user in users)
            {
                count++;
                if (operation == "grant")
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _context.SaveChangesAsync();

                    count -= 1;
                }
                else if (operation == "remove")
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                    await _context.SaveChangesAsync();

                    count -= 1;
                }

                if (count == 1)
                    break;
            }

            if (operation == "block")
                users.ForEach(u => u.IsBlocked = true);
            else if (operation == "unblock")
                users.ForEach(u => u.IsBlocked = false);
            else if (operation == "delete")
            {
                foreach(var user in users)
                {
                    var fullUser = await _context.Users
                        .Include(u => u.OwnerOfInventories)
                            .ThenInclude(f => f.Fields)
                        .Include(u => u.OwnerOfInventories)
                            .ThenInclude(a => a.AccessList)
                        .Include(u => u.OwnerOfInventories)
                            .ThenInclude(i => i.Items)
                        .Include(u => u.OwnerOfInventories)
                            .ThenInclude(t => t.Tags)
                        .FirstOrDefaultAsync(u => u.Id == user.Id);

                    if(fullUser != null)
                    {
                        foreach(var inventory in fullUser.OwnerOfInventories)
                        {
                            _context.InventoryFields.RemoveRange(inventory.Fields);
                            _context.InventoryAccesses.RemoveRange(inventory.AccessList);
                            _context.Items.RemoveRange(inventory.Items);
                            _context.Tags.RemoveRange(inventory.Tags);
                        }

                        _context.Inventories.RemoveRange(fullUser.OwnerOfInventories);
                        _context.Users.Remove(fullUser);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateUserAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var userRole = await _userManager.IsInRoleAsync(user, "Admin");

            if (user.IsBlocked || !userRole)
                return false;
            else
                return true;
        }
    }
}
