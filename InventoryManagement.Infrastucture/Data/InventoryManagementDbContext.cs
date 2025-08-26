using InventoryManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace InventoryManagement.Infrastucture.Data
{
    public class InventoryManagementDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryField> InventoryFields { get; set; }
        public DbSet<InventoryAccess> InventoryAccesses { get; set; }
        public DbSet<InventorySequence> InventorySequences { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemFieldValue> ItemFieldValues { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<FieldUsageStat> FieldUsageStat { get; set; }

        public InventoryManagementDbContext(DbContextOptions<InventoryManagementDbContext> opts)
            : base(opts)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Inventory>()
                .ToTable("Inventories");
            builder.Entity<Inventory>()
                .HasKey(x => x.Id);
            builder.Entity<Inventory>()
                .Property(x => x.Version)
                .IsRowVersion();
            builder.Entity<Inventory>()
                .HasMany(i => i.Tags)
                .WithMany(t => t.Inventories);

            builder.Entity<Item>()
                .ToTable("Items");
            builder.Entity<Item>()
                .HasKey(x => x.Id);
            builder.Entity<Item>()
                .Property(x => x.Version)
                .IsRowVersion();
            builder.Entity<Item>()
                .HasIndex(i => new { i.InventoryId, i.CustomId })
                .IsUnique();
            builder.Entity<Item>()
                .HasOne(a => a.Inventory)
                .WithMany(i => i.Items)
                .HasForeignKey(a => a.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InventoryField>()
                .ToTable("InventoryFields");
            builder.Entity<InventoryField>()
                .HasKey(x => x.Id);
            builder.Entity<InventoryField>()
                .HasOne(i => i.Inventory)
                .WithMany(x => x.Fields)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InventoryAccess>()
                .ToTable("InventoryAccesses");
            builder.Entity<InventoryAccess>()
                .HasKey(x => x.Id);
            builder.Entity<InventoryAccess>()
                .HasOne(a => a.Inventory)
                .WithMany(i => i.AccessList)
                .HasForeignKey(a => a.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<InventoryAccess>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InventorySequence>()
                .ToTable("InventorySequences");
            builder.Entity<InventorySequence>()
                .HasKey(x => x.Id);

            builder.Entity<ItemFieldValue>()
                .ToTable("FieldValues");
            builder.Entity<ItemFieldValue>()
                .HasKey(x => x.Id);
            builder.Entity<ItemFieldValue>()
                .HasOne(v => v.Item)
                .WithMany(i => i.FieldValues)
                .HasForeignKey(v => v.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .ToTable("Posts");
            builder.Entity<Post>()
                .HasKey(x => x.Id);

            builder.Entity<Tag>()
                .ToTable("Tags");
            builder.Entity<Tag>()
                .HasKey(x => x.Id);

            builder.Entity<FieldUsageStat>()
                .ToTable("FieldStat");
            builder.Entity<FieldUsageStat>()
                .HasKey(x => x.Id);

            base.OnModelCreating(builder);
        }
    }
}
