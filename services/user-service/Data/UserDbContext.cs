using Microsoft.EntityFrameworkCore;
using user_service.Models;

namespace user_service.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        // User Domain DbSets
        public DbSet<CustomerProfile> CustomerProfiles { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;

        // Product Domain DbSets
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductVariant> ProductVariants { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<ProductReview> ProductReviews { get; set; } = null!;

        // Cart & Order Domain DbSets
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Domain Mappings
            modelBuilder.Entity<CustomerProfile>(entity =>
            {
                entity.HasIndex(e => e.UserId).IsUnique();
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasOne(a => a.CustomerProfile)
                      .WithMany(c => c.Addresses)
                      .HasForeignKey(a => a.CustomerProfileId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Product Domain Mappings
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Brand)
                      .WithMany(b => b.Products)
                      .HasForeignKey(p => p.BrandId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasIndex(e => e.Sku).IsUnique();

                entity.HasOne(pv => pv.Product)
                      .WithMany(p => p.Variants)
                      .HasForeignKey(pv => pv.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasOne(pi => pi.Product)
                      .WithMany(p => p.Images)
                      .HasForeignKey(pi => pi.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductReview>(entity =>
            {
                entity.HasOne(pr => pr.Product)
                      .WithMany(p => p.Reviews)
                      .HasForeignKey(pr => pr.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Cart Mappings
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasIndex(c => c.CustomerId).IsUnique();
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(ci => ci.Cart)
                      .WithMany(c => c.Items)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.ProductVariant)
                      .WithMany()
                      .HasForeignKey(ci => ci.ProductVariantId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Order Mappings
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(o => o.OrderNumber).IsUnique();

                entity.HasOne(o => o.Customer)
                      .WithMany()
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.ShippingAddress)
                      .WithMany()
                      .HasForeignKey(o => o.AddressId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.ProductVariant)
                      .WithMany()
                      .HasForeignKey(oi => oi.ProductVariantId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderStatusHistory>(entity =>
            {
                entity.HasOne(osh => osh.Order)
                      .WithMany(o => o.StatusHistories)
                      .HasForeignKey(osh => osh.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
