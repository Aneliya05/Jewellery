using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace EllaJewelry.Infrastructure.Data
{
    public class EllaJewelryDbContext : IdentityDbContext<User>
    {
        public EllaJewelryDbContext()
        {
            
        }
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

        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EllaJewelryDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
            .ToTable(t => t.HasCheckConstraint("CK_YourEntity_ProductOrService",
                "(ProductID IS NOT NULL OR ServiceID IS NOT NULL) AND NOT (ProductID IS NOT NULL AND ServiceID IS NOT NULL)"));
            
            ConfigureEnumConversions(modelBuilder);
            ConfigureRelationships(modelBuilder);
            Seeding(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        #region Configurations
        private void ConfigureEnumConversions(ModelBuilder modelBuilder)
        {
            // Order
            modelBuilder.Entity<Order>().Property(r => r.Shipping).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(r => r.PaymentMethod).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(r => r.Status).HasConversion<string>();

            // Element
            modelBuilder.Entity<Element>().Property(r => r.ItemCategory).HasConversion<string>();
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            //Product
            modelBuilder.Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Product Images
            modelBuilder.Entity<ProductImage>()
               .HasOne(c => c.Product)
               .WithMany(c => c.Images)
               .HasForeignKey(c => c.ProductID)
               .OnDelete(DeleteBehavior.Cascade);

            // Element
            modelBuilder.Entity<Element>()
                .HasOne(c => c.PersonalizedProduct)
                .WithMany(c => c.Elements)
                .HasForeignKey(c => c.PersonalizedProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Element>()
                .HasOne(c => c.Service)
                .WithMany(c => c.Elements)
                .HasForeignKey(c => c.ServiceID)
                .OnDelete(DeleteBehavior.Cascade);

            // Order Item
            modelBuilder.Entity<OrderItem>()
                .HasOne(c => c.Order)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(c => c.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(c => c.Product)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(c => c.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(c => c.Service)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(c => c.ServiceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
        #endregion

        #region Seeding
        private void Seeding(ModelBuilder modelBuilder)
        {
            SeedRoles(modelBuilder);
            SeedAdminUser(modelBuilder);
        }
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );
        }

        private void SeedAdminUser(ModelBuilder modelBuilder)
        {
            var adminUserName = Environment.GetEnvironmentVariable("ADMIN_USERNAME") ?? "localadmin";
            var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "localadmin@example.com";
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "LocalPassword123!";
            var adminFirstName = Environment.GetEnvironmentVariable("ADMIN_FIRSTNAME") ?? "Local";
            var adminLastName = Environment.GetEnvironmentVariable("ADMIN_LASTNAME") ?? "Admin";
            var adminPhone = Environment.GetEnvironmentVariable("ADMIN_PHONE") ?? "+0000000000";

            var adminUser = new User
            {
                Id = "1",
                UserName = adminUserName,
                NormalizedUserName = adminUserName?.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail?.ToUpper(),
                EmailConfirmed = true,
                FirstName = adminFirstName,
                LastName = adminLastName,
                PhoneNumber = adminPhone,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, adminPassword),
                SecurityStamp = string.Empty
            };

            var ownerUserName = Environment.GetEnvironmentVariable("OWNER_USERNAME") ?? "localowner";
            var ownerEmail = Environment.GetEnvironmentVariable("OWNER_EMAIL") ?? "localowner@example.com";
            var ownerPassword = Environment.GetEnvironmentVariable("OWNER_PASSWORD") ?? "LocalPassword123456!";
            var ownerFirstName = Environment.GetEnvironmentVariable("OWNER_FIRSTNAME") ?? "Local";
            var ownerLastName = Environment.GetEnvironmentVariable("OWNER_LASTNAME") ?? "Owner";
            var ownerPhone = Environment.GetEnvironmentVariable("OWNER_PHONE") ?? "+0000000001";
            
            var ownerUser = new User
            {
                Id = "2",
                UserName = ownerUserName,
                NormalizedUserName = ownerUserName?.ToUpper(),
                Email = ownerEmail,
                NormalizedEmail = ownerEmail?.ToUpper(),
                EmailConfirmed = true,
                FirstName = ownerFirstName,
                LastName = ownerLastName,
                PhoneNumber = ownerPhone,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, ownerPassword),
                SecurityStamp = string.Empty
            };

            modelBuilder.Entity<User>().HasData(adminUser);
            modelBuilder.Entity<User>().HasData(ownerUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = "1" }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = ownerUser.Id, RoleId = "1" }
            );
        }
        #endregion
    }
}
