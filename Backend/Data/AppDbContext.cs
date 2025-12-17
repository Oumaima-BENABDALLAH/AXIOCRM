using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Models;
using ProductManager.API.Models.AuthentificationJWT;
using ProductManager.API.Models.Invoice;
using ProductManager.API.Models.Scheduler;

namespace ProductManager.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Client> Clients => Set<Client>();
       
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
        public DbSet<DeliveryMethod> DeliveryMethods => Set<DeliveryMethod>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
        public DbSet<ScheduleEvent> ScheduleEvents => Set<ScheduleEvent>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        
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
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(i => i.OrderId);
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)       // chaque item a une facture
                .WithMany(i => i.Items)         // chaque facture a plusieurs items
                .HasForeignKey(ii => ii.InvoiceId) // clé étrangère dans InvoiceItem
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
