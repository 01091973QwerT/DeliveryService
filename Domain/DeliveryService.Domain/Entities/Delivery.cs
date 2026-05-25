using DeliveryService.Domain.Base;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Доставка (deliveries).
///
/// Основная сущность сервиса доставки:
/// - связывает отправителя и получателя;
/// - хранит адреса забора и доставки;
/// - управляет статусом и способом оплаты;
/// - хранит коллекцию уведомлений.
/// </summary>
public class Delivery : Entity<Guid>
{
    private readonly ICollection<DeliveryNotification> _notifications = [];

    public Sender Sender { get; private set; } = default!;
    public Receiver Receiver { get; private set; } = default!;

    public Address PickupAddress { get; private set; } = default!;
    public Address DeliveryAddress { get; private set; } = default!;

    public DeliveryStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public bool ReceiverPassportShown { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<DeliveryNotification> Notifications => _notifications.ToList().AsReadOnly();

    /// <summary>
    /// Активна ли доставка (не отменена и не завершена).
    /// </summary>
    public bool IsActive => Status == DeliveryStatus.Pending || Status == DeliveryStatus.InProgress;

    protected Delivery()
    {
    }

    internal Delivery(
        Sender sender,
        Receiver receiver,
        Address pickupAddress,
        Address deliveryAddress,
        PaymentMethod paymentMethod,
        bool receiverPassportShown,
        DateTime createdAtUtc
    ) : this(
        Guid.NewGuid(),
        sender,
        receiver,
        pickupAddress,
        deliveryAddress,
        DeliveryStatus.Pending,
        paymentMethod,
        receiverPassportShown,
        createdAtUtc,
        createdAtUtc
    )
    {
        if (sender is null) throw new ArgumentNullValueException(nameof(sender));
        if (receiver is null) throw new ArgumentNullValueException(nameof(receiver));
        ValidateDeliveryData(pickupAddress, deliveryAddress);
    }

    protected Delivery(
        Guid id,
        Sender sender,
        Receiver receiver,
        Address pickupAddress,
        Address deliveryAddress,
        DeliveryStatus status,
        PaymentMethod paymentMethod,
        bool receiverPassportShown,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(id)
    {
        if (sender is null) throw new ArgumentNullValueException(nameof(sender));
        if (receiver is null) throw new ArgumentNullValueException(nameof(receiver));
        if (pickupAddress is null) throw new ArgumentNullValueException(nameof(pickupAddress));
        if (deliveryAddress is null) throw new ArgumentNullValueException(nameof(deliveryAddress));

        Sender = sender;
        Receiver = receiver;
        PickupAddress = pickupAddress;
        DeliveryAddress = deliveryAddress;
        Status = status;
        PaymentMethod = paymentMethod;
        ReceiverPassportShown = receiverPassportShown;
        CreatedAt = createdAt.Kind == DateTimeKind.Utc ? createdAt : DateTime.SpecifyKind(createdAt, DateTimeKind.Utc);
        UpdatedAt = updatedAt.Kind == DateTimeKind.Utc ? updatedAt : DateTime.SpecifyKind(updatedAt, DateTimeKind.Utc);
    }

    private static void ValidateDeliveryData(Address pickupAddress, Address deliveryAddress)
    {
        if (pickupAddress is null)
            throw new DeliveryDataValidationException(nameof(pickupAddress), null);
        if (deliveryAddress is null)
            throw new DeliveryDataValidationException(nameof(deliveryAddress), null);
        if (pickupAddress.Value == deliveryAddress.Value)
            throw new DeliveryDataValidationException("addresses", "Pickup and delivery addresses cannot be the same");
    }

    /// <summary>
    /// Отменяет доставку.
    /// </summary>
    internal bool Cancel(Sender actor)
    {
        if (actor is null) throw new ArgumentNullValueException(nameof(actor));

        if (Status == DeliveryStatus.Delivered)
            throw new InvalidDeliveryStatusException(this, Status, "Отмена доставки");

        if (Status == DeliveryStatus.Cancelled)
            return false;

        Status = DeliveryStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;

        // Добавляем уведомление об отмене
        AddNotification($"Доставка отменена отправителем «{actor.Name.Value}».");
        return true;
    }

    /// <summary>
    /// Изменяет способ оплаты.
    /// </summary>
    internal bool ChangePaymentMethod(Sender actor, PaymentMethod newPaymentMethod)
    {
        if (actor is null) throw new ArgumentNullValueException(nameof(actor));

        if (!IsActive)
            throw new InvalidDeliveryStatusException(this, Status, "Изменение способа оплаты");

        if (PaymentMethod == newPaymentMethod)
            return false;

        PaymentMethod = newPaymentMethod;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    /// <summary>
    /// Запускает доставку (переводит в статус "В процессе").
    /// </summary>
    public bool StartDelivery()
    {
        if (Status != DeliveryStatus.Pending)
            throw new InvalidDeliveryStatusException(this, Status, "Начало доставки");

        Status = DeliveryStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;

        AddNotification("Доставка начата, курьер в пути.");
        return true;
    }

    /// <summary>
    /// Завершает доставку.
    /// </summary>
    public bool CompleteDelivery()
    {
        if (Status != DeliveryStatus.InProgress)
            throw new InvalidDeliveryStatusException(this, Status, "Завершение доставки");

        Status = DeliveryStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;

        AddNotification("Доставка успешно завершена.");
        return true;
    }

    /// <summary>
    /// Use case: «Уведомление о доставке».
    /// Добавляет уведомление для отправителя и получателя.
    /// </summary>
    public DeliveryNotification AddNotification(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentNullValueException(nameof(message));

        var notification = DeliveryNotification.Create(this, message, DateTime.UtcNow);
        _notifications.Add(notification);
        return notification;
    }

    /// <summary>
    /// Устанавливает флаг показа паспорта получателя.
    /// </summary>
    public void SetReceiverPassportShown(bool shown)
    {
        ReceiverPassportShown = shown;
        UpdatedAt = DateTime.UtcNow;
    }
}
