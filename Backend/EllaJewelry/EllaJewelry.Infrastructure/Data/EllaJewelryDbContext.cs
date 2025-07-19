using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace EllaJewelry.Infrastructure.Data
{
    public class EllaJewelryDbContext : IdentityDbContext<User>
    {
        public EllaJewelryDbContext(DbContextOptions<EllaJewelryDbContext> options) : base(options) { }

        #region DbSets
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PersonalizedProduct> PersonalizedProducts { get; set;}
        public DbSet<Element> Elements { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<OrderService> ServiceOrders { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureEnumConversions(ModelBuilder modelBuilder)
        {
            // Product
            modelBuilder.Entity<Product>().Property(r => r.Category).HasConversion<string>();

            //Order
            modelBuilder.Entity<Order>().Property(r => r.Shipping).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(r => r.PaymentMethod).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(r => r.Status).HasConversion<string>();

            
        }
    }
}
