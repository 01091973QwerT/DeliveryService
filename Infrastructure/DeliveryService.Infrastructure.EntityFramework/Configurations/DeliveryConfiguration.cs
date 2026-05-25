using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.ValueObjects;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.Infrastructure.EntityFramework.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("deliveries");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<Guid>("SenderId")
            .HasColumnName("sender_id")
            .IsRequired();

        builder.Property<Guid>("ReceiverId")
            .HasColumnName("receiver_id")
            .IsRequired();

        builder.Property(x => x.PickupAddress)
            .HasColumnName("pickup_address")
            .IsRequired()
            .HasConversion(
                addr => addr.Value,
                str => new Address(str)
            )
            .HasColumnType("text");

        builder.Property(x => x.DeliveryAddress)
            .HasColumnName("delivery_address")
            .IsRequired()
            .HasConversion(
                addr => addr.Value,
                str => new Address(str)
            )
            .HasColumnType("text");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion(
                s => s.ToString().ToLower(),
                str => Enum.Parse<DeliveryStatus>(str, true)
            )
            .HasColumnType("text");

        builder.Property(x => x.PaymentMethod)
            .HasColumnName("payment_method")
            .IsRequired()
            .HasConversion(
                p => p.ToString().ToLower(),
                str => Enum.Parse<PaymentMethod>(str, true)
            )
            .HasColumnType("text");

        builder.Property(x => x.ReceiverPassportShown)
            .HasColumnName("receiver_passport_shown")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne(x => x.Sender)
            .WithMany("_deliveries")
            .HasForeignKey("SenderId")
            .IsRequired();

        builder.HasOne(x => x.Receiver)
            .WithMany()
            .HasForeignKey("ReceiverId")
            .IsRequired();

        builder.HasMany<DeliveryNotification>("_notifications")
            .WithOne(x => x.Delivery)
            .HasForeignKey("DeliveryId")
            .HasPrincipalKey(x => x.Id);

        builder.Ignore(x => x.Notifications);
        builder.Ignore(x => x.IsActive);
    }
}