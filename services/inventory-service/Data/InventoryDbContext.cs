using Microsoft.EntityFrameworkCore;
using inventory_service.Models;

namespace inventory_service.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Inventory> Inventories { get; set; } = null!;
        public DbSet<StockReservation> StockReservations { get; set; } = null!;
        public DbSet<StockTransaction> StockTransactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.ProductVariantId);
            });
            
        }
    }
}