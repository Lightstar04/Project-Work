using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Interfaces;
using InventoryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services.Classes
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryManagementDbContext _context;
        private readonly INotificationService _notificationService;

        public InventoryService(InventoryManagementDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<InventoryDetailsViewModel> GetByIdAsync(int id)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Fields)
                .FirstOrDefaultAsync(i => i.Id == id);

            var items = await _context.Items
                .Include(f => f.FieldValues)
                .Where(i => i.InventoryId == id)
                .ToListAsync();

            var posts = await _context.Posts.Where(p => p.InventoryId == id).ToListAsync();
            var viewModel = new InventoryDetailsViewModel{ Inventory = inventory, Items = items, Posts = posts };

            return viewModel;
        }
        
        public async Task<Inventory> CreateAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            
            _context.InventorySequences.Add(new InventorySequence 
            {
                InventoryId = inventory.Id, 
                NextValue = 1 
            });
            
            await _context.SaveChangesAsync();
            
            return inventory;
        }

        public async Task UpdateAsync(int id, Action<Inventory> applyChanges, byte[] version)
        {
            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory is null)
            {
                throw new KeyNotFoundException();                     
            }

            if(version is not null)
            {
                inventory.Version = version;
            }

            applyChanges(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);

            if(inventory is null)
            {
                throw new KeyNotFoundException();
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SearchAsync(string query)
        {
            var baseQuery = _context.Inventories.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(i => i.Title.Contains(query) || i.Description == null && i.Description.Contains(query));
            }

            var total = await baseQuery.CountAsync();

            return total;
        }

        public async Task SaveAsync(
            int id, string title, 
            string description, string rowVersionBase64)
        {
            byte[] rowVersion = null;

            if(!string.IsNullOrEmpty(rowVersionBase64))
            {
                rowVersion = Convert.FromBase64String(rowVersionBase64);
            }

            await UpdateAsync(id, i =>
            {
                i.Title = title;
                i.Description = description;
            }, rowVersion);
        }

        public async Task SaveAsync(int id, string customIdTemplateJson, string rowVersionBase64)
        {
            byte[] rowVersion = null;
            if(!string.IsNullOrEmpty(rowVersionBase64))
            {
                rowVersion = Convert.FromBase64String(rowVersionBase64);
            }    

            await UpdateAsync(id, i =>
            {
                i.CustomIdTemplateJson = customIdTemplateJson;
            }, rowVersion);
        }

        public async Task PostAsync(Post post, int id)
        {
            _context.Posts.Add(post);
            
            await _context.SaveChangesAsync();
            
            await _notificationService.NotifyPostAsync(id, new { author = post.AuthorId, markdown = post.Markdown });
        }

    }
}
