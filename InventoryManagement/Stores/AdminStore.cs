using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastucture.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Stores
{
    public class AdminStore
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly InventoryManagementDbContext _context;

        public AdminStore(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, InventoryManagementDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<AppUser>> Get()
        {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }

        public async Task ButtonOperation(string[] selected, string operation)
        {
            var ids = selected.ToList();
            var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

            int count = 0;

            foreach (var user in users)
            {
                count++;
                if(operation == "grant")
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _context.SaveChangesAsync();
                    
                    count -= 1;
                }
                else if(operation == "remove")
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
                _context.Users.RemoveRange(users);

                await _context.SaveChangesAsync();
        }
    }
}
