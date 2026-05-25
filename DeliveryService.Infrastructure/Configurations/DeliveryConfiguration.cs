using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.ValueObjects;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.Infrastructure.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("deliveries");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.PickupAddress)
            .HasColumnName("pickup_address")
            .IsRequired()
            .HasConversion(
                address => address.Value,
                str => new AddressText(str)
            )
            .HasMaxLength(AddressValidator.MAX_LENGTH);

        builder.Property(x => x.DeliveryAddress)
            .HasColumnName("delivery_address")
            .IsRequired()
            .HasConversion(
                address => address.Value,
                str => new AddressText(str)
            )
            .HasMaxLength(AddressValidator.MAX_LENGTH);

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion(
                s => s == DeliveryStatus.Created ? "created"
                    : s == DeliveryStatus.InTransit ? "in_transit"
                    : s == DeliveryStatus.Delivered ? "delivered"
                    : "cancelled",
                str => str == "created" ? DeliveryStatus.Created
                    : str == "in_transit" ? DeliveryStatus.InTransit
                    : str == "delivered" ? DeliveryStatus.Delivered
                    : DeliveryStatus.Cancelled
            );

        builder.Property(x => x.PaymentMethod)
            .HasColumnName("payment_method")
            .IsRequired()
            .HasConversion(
                p => p == PaymentMethod.None ? "none"
                    : p == PaymentMethod.Online ? "online"
                    : p == PaymentMethod.Cash ? "cash"
                    : p == PaymentMethod.Terminal ? "terminal"
                    : "corporate",
                str => str == "none" ? PaymentMethod.None
                    : str == "online" ? PaymentMethod.Online
                    : str == "cash" ? PaymentMethod.Cash
                    : str == "terminal" ? PaymentMethod.Terminal
                    : PaymentMethod.Corporate
            );

        builder.Property(x => x.PaymentStatus)
            .HasColumnName("payment_status")
            .IsRequired()
            .HasConversion(
                p => p == PaymentStatus.None ? "none"
                    : p == PaymentStatus.Pending ? "pending"
                    : "paid",
                str => str == "none" ? PaymentStatus.None
                    : str == "pending" ? PaymentStatus.Pending
                    : PaymentStatus.Paid
            );

        builder.Property(x => x.ReceiverPassportShown)
            .HasColumnName("receiver_passport_shown");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.HasOne(x => x.Sender)
            .WithMany(s => s.Deliveries)
            .HasForeignKey("SenderId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Receiver)
            .WithMany()
            .HasForeignKey("ReceiverId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
