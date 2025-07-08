using Microsoft.EntityFrameworkCore;
using ProductManager.API.Models;

namespace ProductManager.API.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<ClientProduct> ClientProducts => Set<ClientProduct>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure the many-to-many relationship between Client and Product
            modelBuilder.Entity<ClientProduct>()
                .HasKey(p => new { p.ClientId, p.ProductId});
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasPrecision(18, 2);
            modelBuilder.Entity<ClientProduct>()
                .HasOne(cp => cp.Client)
                .WithMany(c => c.ClientProducts)
                .HasForeignKey(cp => cp.ClientId);
            modelBuilder.Entity<ClientProduct>()
               .HasOne(cp => cp.Product)
               .WithMany(c => c.ClientProducts)
               .HasForeignKey(cp => cp.ProductId);
            //Configure the many-to-many relationship between Order and Product
             modelBuilder.Entity<OrderProduct>()
             .HasKey(op => new { op.OrderId, op.ProductId });

             modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);
        }
    }
}
