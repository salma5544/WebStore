using Microsoft.EntityFrameworkCore;

namespace WebStore.Models
{
    public class ECommerceDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();

        public ECommerceDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>().HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "Gaming Laptop", Price = 1200, Stock = 10 },
                new Product { Id = 2, Name = "Mouse", Description = "Wireless Mouse", Price = 25, Stock = 100 }
            );
        }
    }
}
