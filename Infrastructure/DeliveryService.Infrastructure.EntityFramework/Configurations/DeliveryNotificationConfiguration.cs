using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryService.Domain.Entities;

namespace DeliveryService.Infrastructure.EntityFramework.Configurations;

public class DeliveryNotificationConfiguration : IEntityTypeConfiguration<DeliveryNotification>
{
    public void Configure(EntityTypeBuilder<DeliveryNotification> builder)
    {
        builder.ToTable("delivery_notifications");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<Guid>("DeliveryId")
            .HasColumnName("delivery_id")
            .IsRequired();

        builder.Property(x => x.Message)
            .HasColumnName("message")
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.SentAt)
            .HasColumnName("sent_at")
            .IsRequired();

        builder.HasOne(x => x.Delivery)
            .WithMany("_notifications")
            .HasForeignKey("DeliveryId")
            .IsRequired();
    }
}
