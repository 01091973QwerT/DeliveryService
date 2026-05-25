using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Entities;
using DeliveryService.Infrastructure.Configurations;

namespace DeliveryService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Sender> Senders { get; set; }
    public DbSet<Receiver> Receivers { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<PassportData> PassportData { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SenderConfiguration());
        modelBuilder.ApplyConfiguration(new ReceiverConfiguration());
        modelBuilder.ApplyConfiguration(new PassportDataConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}