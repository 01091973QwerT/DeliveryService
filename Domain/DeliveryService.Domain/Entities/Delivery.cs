using DeliveryService.Domain.Base;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Доставка
/// </summary>
public class Delivery : Entity<Guid>
{
    public Sender Sender { get; private set; }
    public Receiver Receiver { get; private set; }
    public AddressText PickupAddress { get; private set; }
    public AddressText DeliveryAddress { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public bool? ReceiverPassportShown { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Delivery() { }

    public Delivery(
        Sender sender,
        Receiver receiver,
        AddressText pickupAddress,
        AddressText deliveryAddress,
        PaymentMethod paymentMethod)
        : base(Guid.NewGuid())
    {
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        PickupAddress = pickupAddress ?? throw new ArgumentNullException(nameof(pickupAddress));
        DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
        PaymentMethod = paymentMethod;

        Status = DeliveryStatus.Created;
        PaymentStatus = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public bool SelectPaymentMethod(PaymentMethod paymentMethod)
    {
        if (PaymentMethod == paymentMethod)
            return false;

        PaymentMethod = paymentMethod;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool PayOnline()
    {
        if (PaymentStatus != PaymentStatus.Pending)
            return false;

        PaymentStatus = PaymentStatus.Paid;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool StartDelivery()
    {
        if (Status != DeliveryStatus.Created)
            return false;

        if (PaymentStatus != PaymentStatus.Paid)
            return false;

        Status = DeliveryStatus.InTransit;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    /// <summary>
    /// Отметить, что груз забрали (переход в статус PickedUp)
    /// </summary>
    public bool MarkAsPickedUp()
    {
        if (Status != DeliveryStatus.InTransit)
            return false;

        Status = DeliveryStatus.PickedUp;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool ShowPassport()
    {
        if (ReceiverPassportShown == true)
            return false;

        ReceiverPassportShown = true;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool ReceiveDelivery()
    {
        if (Status != DeliveryStatus.PickedUp)
            return false;

        if (ReceiverPassportShown != true)
            return false;

        Status = DeliveryStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    /// <summary>
    /// Отмена доставки возможна только в статусах Created или InTransit.
    /// В статусах PickedUp, Delivered, Cancelled отмена запрещена.
    /// </summary>
    public bool CancelDelivery()
    {
        // Разрешённые статусы для отмены
        if (Status != DeliveryStatus.Created && Status != DeliveryStatus.InTransit)
            return false;

        Status = DeliveryStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }
}