
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventoryManagement.Stores
{
    public class HomeStore
    {
        private readonly InventoryManagementDbContext _context;

        public HomeStore(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<HomeViewModel> Get(string query)
        {
            var inventories = _context.Inventories
                .Include(i => i.Owner)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5);

            if (!string.IsNullOrWhiteSpace(query))
            {
                inventories = _context.Inventories.Where(i => i.Title.Contains(query) || (i.Description != null && i.Description.Contains(query)));
            }

            var model = new HomeViewModel
            {
                Inventories = await inventories.ToListAsync(),
                Top = await _context.Inventories
                            .Include(i => i.Items)
                            .OrderByDescending(i => i.Items.Count)
                            .Take(5)
                            .ToListAsync()
            };

            return model;
        }
    }
}
