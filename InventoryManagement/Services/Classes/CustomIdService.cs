using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Infrastucture.Data;
using InventoryManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryManagement.Services.Classes
{
    public class CustomIdService : ICustomIdService
    {
        private readonly InventoryManagementDbContext _context;
        private readonly Random _random = new Random();

        public CustomIdService(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateForInventoryAsync(int inventoryId, CancellationToken token = default)
        {
            var inventory = await _context.Inventories.FindAsync(new object[] { inventoryId }, token);
            
            if (inventory == null)
                throw new InvalidOperationException("Inventory not found");

            var elements = JsonSerializer.Deserialize<List<CustomIdElement>>(inventory.CustomIdTemplateJson ?? "[]") ?? new();

            using var transaction = await _context.Database.BeginTransactionAsync(token);
            var parts = new List<string>();

            foreach(var element in elements)
            {
                parts.Add(await RenderElementAsync(element, inventoryId, token));
            }

            var candidate = string.Concat(parts);

            int tries = 0;
            
            while(await _context.Items.AnyAsync(i => i.InventoryId == inventoryId && i.CustomId == candidate, token))
            {
                tries++;
                if (tries > 5)
                    throw new Exception("Unable to generate unique custom id");

                parts = new List<string>();
                foreach(var element in elements)
                {
                    parts.Add(await RenderElementAsync(element, inventoryId, token));
                }
                candidate = string.Concat(parts);
            }

            await transaction.CommitAsync(token);
            
            return candidate;
        }

        private async Task<string> RenderElementAsync(CustomIdElement element, int inventoryId, CancellationToken token)
        {
            switch (element.Type)
            {
                case CustomIdElementType.FixedText:
                    return $"{element.Parameter}{_random.Next(0, 1000).ToString("D4")}" ?? "";
                case CustomIdElementType.Random20Bit:
                    return _random.Next(0, 1 << 20).ToString();
                case CustomIdElementType.Random32Bit:
                    return ((uint)_random.Next()).ToString();
                case CustomIdElementType.Random6Digit:
                    return _random.Next(0, 1000000).ToString("D6");
                default:
                    return "";
            }
        }

        public string RenderPreview(string jsonTemplate)
        {
            try
            {
                var elements = JsonSerializer.Deserialize<List<CustomIdElement>>(jsonTemplate ?? "[]") ?? new();

                var parts = elements.Select(e => e.Type switch
                {
                    CustomIdElementType.FixedText => e.Parameter ?? "T",
                    CustomIdElementType.Random20Bit => "R20",
                    CustomIdElementType.Random32Bit => "R32",
                    CustomIdElementType.Random6Digit => "000123",
                    _ => "?"
                });
                return string.Concat(parts);
            }
            catch
            {
                return "(invalid)";
            }
        }

        public bool ValidateFormat(string jsonTemplate, string candidate)
        {
            if (string.IsNullOrEmpty(candidate) || candidate.Length > 200)
                return false;
            
            return true;
        }
    }
}
