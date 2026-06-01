

using AXIOCRM.Domain.Email;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Domain.Entities.Invoice;
using AXIOCRM.Domain.Entities.Kanban;
using AXIOCRM.Domain.Entities.Notification;
using AXIOCRM.Domain.Entities.Scheduler;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AXIOCRM.Infrastructure.Persistence
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
        public DbSet<EmailLog> EmailLogs => Set<EmailLog>();
        public DbSet<AdminNotification> AdminNotifications { get; set; }
        public DbSet<BookingTask> BookingTasks => Set<BookingTask>();
        public DbSet<ClientFinancialSummary> ClientFinancialSummaries { get; set; }
        public DbSet<InactiveClient> InactiveClients { get; set; }
        public DbSet<ClientInvoiceRank> ClientInvoiceRanks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             modelBuilder.Entity<OrderProduct>()
             .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(i => i.OrderId);
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)      
                .WithMany(i => i.Items)        
                .HasForeignKey(ii => ii.InvoiceId) 
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ClientFinancialSummary>()
                .ToView("vw_ClientFinancialSummary")
                .HasKey(c => c.ClientId);
            modelBuilder.Entity<InactiveClient>(entity =>
            {
                entity.HasNoKey();
               // entity.ToView("vw_InactiveClients");
             });
            modelBuilder.Entity<ClientInvoiceRank>(entity =>
                {
                    entity.HasNoKey();
                    //entity.ToView("vw_ClientInvoiceRank");
                });

        }
    }
}
