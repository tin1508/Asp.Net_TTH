using StationeryStore.Mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StationeryStore.Mvc.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //stationeries table
        public DbSet<Stationery> Stationeries {get; set;} = null!;
        //categories table
        public DbSet<Category> Categories {get; set;} = null!;
        //stationery orders table 
        public DbSet<StationeryOrder> StationeryOrders {get; set;} = null!;
        //order detail table
        public DbSet<OrderDetail> OrderDetails {get; set;} = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Cart> Carts {get; set;} = null!;
        public DbSet<CartItem> CartItems {get; set;} = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<Stationery>(entity =>
            {
                entity.Property(s => s.Price).HasColumnType("decimal(10,2)");
                entity.HasIndex(s => s.Sku).IsUnique();
                entity.HasQueryFilter(s => !s.IsDeleted);
            });
            modelBuilder.Entity<StationeryOrder>(entity =>
            {
                entity.Property(s => s.TotalAmount).HasColumnType("decimal(10,2)");
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(o => o.UnitPrice).HasColumnType("decimal(10,2)");
                entity.HasOne(o => o.Stationery).WithMany().IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Cart>(entity =>
            {
               entity.HasIndex(c => c.UserId).IsUnique(); 
            });
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasQueryFilter(c => c.Stationery.IsDeleted == false);
            });
        }
    }
}