using StationeryStore.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace StationeryStore.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //stationeries table
        public DbSet<Stationery> Stationeries {get; set;} = null!;
        //categories table
        public DbSet<Category> Categories {get; set;} = null!;
    }
}