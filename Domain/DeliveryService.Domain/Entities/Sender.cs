using DeliveryService.Domain.Base;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;
using System.Numerics;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Отправитель
/// Связи:
/// - один Sender может иметь один PassportData
/// - один Sender может иметь много Delivery
/// </summary>
public class Sender : Entity<Guid>
{
    // Value Objects
    public Name Name { get; private set; }
    public Phone Phone { get; private set; }

    // Внешний ключ к PassportData
    public Guid? PassportId { get; private set; }

    // Навигационное свойство (связь с паспортными данными)
    public PassportData? PassportData { get; private set; }

    // Коллекция доставок отправителя (ICollection)
    private readonly ICollection<Delivery> _deliveries = new List<Delivery>();
    public IReadOnlyCollection<Delivery> Deliveries => _deliveries.ToList().AsReadOnly();

    private Sender() { }

    public Sender(Name name, Phone phone, PassportData? passportData = null)
        : base(Guid.NewGuid())  // ← исправлено: Guid.NewGuid() (с большой буквы G)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));

        if (passportData != null)
        {
            PassportData = passportData;
            PassportId = passportData.Id;
        }
    }

    /// <summary>
    /// Use Case: Формирование доставки из пункта А в пункт Б
    /// </summary>
    public Delivery CreateDelivery(
        Receiver receiver,
        AddressText pickupAddress,
        AddressText deliveryAddress,
        PaymentMethod paymentMethod)
    {
        if (receiver == null)
            throw new ArgumentNullException(nameof(receiver));

        var delivery = new Delivery(this, receiver, pickupAddress, deliveryAddress, paymentMethod);
        _deliveries.Add(delivery);
        receiver.AddDelivery(delivery);
        return delivery;
    }

    /// <summary>
    /// Use Case: Отмена доставки (только свои)
    /// </summary>
    public void CancelDelivery(Delivery delivery)
    {
        if (delivery == null)
            throw new ArgumentNullException(nameof(delivery));

        if (delivery.Sender.Id != Id)
            throw new AnotherUserEditDeliveryException(delivery.Id, Id, delivery.Sender.Id);

        if (!_deliveries.Contains(delivery))
            throw new DeliveryNotBelongSenderException(delivery.Id, Id, Name.Value);

        delivery.CancelDelivery();
    }

    public void Update(Name? name = null, Phone? phone = null, PassportData? passportData = null)
    {
        if (name != null) Name = name;
        if (phone != null) Phone = phone;
        if (passportData != null)
        {
            PassportData = passportData;
            PassportId = passportData.Id;
        }
    }
}