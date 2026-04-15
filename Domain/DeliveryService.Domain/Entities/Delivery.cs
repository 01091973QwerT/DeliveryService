using DeliveryService.Domain.Base;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Доставка
/// Связи: 
/// - многие Delivery к одному Sender
/// - многие Delivery к одному Receiver
/// </summary>
public class Delivery : Entity<Guid>
{
    // Внешние ключи и навигационные свойства
    public Guid SenderId { get; private set; }
    public Sender Sender { get; private set; }

    public Guid ReceiverId { get; private set; }
    public Receiver Receiver { get; private set; }

    // Value Objects для адресов
    public AddressText PickupAddress { get; private set; }
    public AddressText DeliveryAddress { get; private set; }

    // Статусы (Enums)
    public DeliveryStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }

    // Демонстрация паспорта получателем
    public bool? ReceiverPassportShown { get; private set; }

    // Временные метки
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Delivery() { }

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

        SenderId = sender.Id;
        ReceiverId = receiver.Id;
        Status = DeliveryStatus.Created;
        PaymentStatus = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Use Case: Выбор способа оплаты
    /// </summary>
    public void SelectPaymentMethod(PaymentMethod paymentMethod)
    {
        PaymentMethod = paymentMethod;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Use Case: Онлайн оплата
    /// </summary>
    public void PayOnline()
    {
        if (PaymentStatus != PaymentStatus.Pending)
            throw new InvalidDeliveryStatusException($"Невозможно оплатить доставку {Id}. Текущий статус оплаты: {PaymentStatus}");

        PaymentStatus = PaymentStatus.Paid;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Use Case: Наличные при получении
    /// </summary>
    public void PayByCashOnDelivery()
    {
        if (PaymentMethod != PaymentMethod.Cash)
            throw new InvalidOperationException($"Оплата наличными доступна только при выборе способа оплаты 'Наличные'. Текущий способ: {PaymentMethod}");

        // Статус оплаты остается Pending до момента получения
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Use Case: Терминал при получении
    /// </summary>
    public void PayByTerminalOnDelivery()
    {
        if (PaymentMethod != PaymentMethod.Terminal)
            throw new InvalidOperationException($"Оплата терминалом доступна только при выборе способа оплаты 'Терминал'. Текущий способ: {PaymentMethod}");

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Use Case: Начать доставку (только после оплаты)
    /// </summary>
    public void StartDelivery()
    {
        if (Status != DeliveryStatus.Created)
            throw new InvalidDeliveryStatusException($"Невозможно начать доставку {Id}. Текущий статус: {Status}. Ожидаемый статус: {DeliveryStatus.Created}");

        if (PaymentStatus != PaymentStatus.Paid)
            throw new DeliveryNotPaidException(Id, PaymentStatus.ToString());

        Status = DeliveryStatus.InTransit;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Use Case: Демонстрация паспорта получателем
    /// </summary>
    public void ShowPassport()
    {
        ReceiverPassportShown = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Use Case: Получение доставки (только после демонстрации паспорта)
    /// </summary>
    public void ReceiveDelivery()
    {
        if (Status != DeliveryStatus.InTransit)
            throw new InvalidDeliveryStatusException($"Невозможно получить доставку {Id}. Текущий статус: {Status}. Ожидаемый статус: {DeliveryStatus.InTransit}");

        if (ReceiverPassportShown != true)
            throw new PassportNotShownException(Id);

        Status = DeliveryStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Use Case: Отмена доставки (только если не доставлена)
    /// </summary>
    public void CancelDelivery()
    {
        if (Status == DeliveryStatus.Delivered)
            throw new InvalidDeliveryStatusException($"Невозможно отменить доставку {Id}. Доставка уже получена. Текущий статус: {Status}");

        if (Status == DeliveryStatus.Cancelled)
            throw new InvalidDeliveryStatusException($"Невозможно отменить доставку {Id}. Доставка уже отменена. Текущий статус: {Status}");

        Status = DeliveryStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}
