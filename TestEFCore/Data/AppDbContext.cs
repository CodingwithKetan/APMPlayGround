using Microsoft.EntityFrameworkCore;
using TestEFCore.Models;

namespace TestEFCore.Data;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet for products.
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the Product entity and its primary key.
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
        }
    }
