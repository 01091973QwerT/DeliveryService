using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Entities;

namespace DeliveryService.Infrastructure.EntityFramework;

/// <summary>
/// Контекст базы данных (EF Core).
/// 
/// Здесь определяются "таблицы" через DbSet, а также подключаются конфигурации
/// из папки Configurations (маппинг сущностей домена на таблицы PostgreSQL).
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    /// <summary>Таблица passport_datas</summary>
    public DbSet<PassportData> PassportDatas { get; set; }
    /// <summary>Таблица senders</summary>
    public DbSet<Sender> Senders { get; set; }
    /// <summary>Таблица receivers</summary>
    public DbSet<Receiver> Receivers { get; set; }
    /// <summary>Таблица deliveries</summary>
    public DbSet<Delivery> Deliveries { get; set; }
    /// <summary>Таблица delivery_notifications</summary>
    public DbSet<DeliveryNotification> DeliveryNotifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Автоматически подключаем все IEntityTypeConfiguration<T>
        // из этой сборки (см. папку Configurations).
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
