using StationeryStore.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace StationeryStore.Mvc.Data
{
    public class AppDbContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<Stationery>(entity =>
            {
                entity.Property(s => s.Price).HasColumnType("decimal(10,2)");
            });
            modelBuilder.Entity<StationeryOrder>(entity =>
            {
                entity.Property(s => s.TotalAmount).HasColumnType("decimal(10,2)");
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(o => o.UnitPrice).HasColumnType("decimal(10,2)");
            });
        }
    }
}