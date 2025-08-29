using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InventoryManagement.Infrastucture.Data
{
    public class SeedDatabase
    {
        public static async Task EnsureSeedAsync(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            InventoryManagementDbContext context)
        {
            if(!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var admin = await userManager.FindByNameAsync("admin@local");
            
            if(admin == null)
            {
                admin = new AppUser
                {
                    UserName = "admin@local",
                    Email = "admin@local",
                    DisplayName = "Administrator",
                    Theme = "light"
                };

                await userManager.CreateAsync(admin, "Ozbek12!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if(!await context.Inventories.AnyAsync())
            {
                var inventory = new Inventory
                {
                    Title = "Office equipment",
                    Description = "A sample inventory of office equipment",
                    Category = "Equipment",
                    OwnerId = admin.Id,
                    IsPublic = true,
                    CustomIdTemplateJson = JsonSerializer.Serialize(new[]
                    {
                        new CustomIdElement{Type = CustomIdElementType.FixedText, Parameter = "EQ-"},
                        new CustomIdElement{Type = CustomIdElementType.Sequence, Parameter = "4"}
                    }),
                    CreatedAt = DateTime.Now
                };

                context.Inventories.Add(inventory);
                await context.SaveChangesAsync();

                context.InventoryFields.Add(new InventoryField
                {
                    InventoryId = inventory.Id,
                    Title = "Model",
                    FieldType = InventoryFieldType.SingleLine,
                    ShowInTable = true,
                    Order = 1
                });

                context.InventoryFields.Add(new InventoryField
                {
                    InventoryId = inventory.Id,
                    Title = "Price",
                    FieldType = InventoryFieldType.Number,
                    ShowInTable = true,
                    Order = 2
                });

                await context.SaveChangesAsync();

                context.InventorySequences.Add(new InventorySequence
                {
                    InventoryId = inventory.Id,
                    NextValue = 1
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
