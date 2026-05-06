using DeliveryService.Domain.Base;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

public class Sender : Entity<Guid>
{
    public Name Name { get; private set; }
    public Phone Phone { get; private set; }
    public PassportData? PassportData { get; private set; }

    private readonly List<Delivery> _deliveries = new();
    public IReadOnlyCollection<Delivery> Deliveries => _deliveries.AsReadOnly();

    protected Sender() { }

    public Sender(Name name, Phone phone, PassportData? passportData = null)
        : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
        PassportData = passportData;
    }

    public bool Update(Name? newName = null, Phone? newPhone = null, PassportData? newPassportData = null)
    {
        bool isUpdated = false;
        if (newName != null && Name != newName) { Name = newName; isUpdated = true; }
        if (newPhone != null && Phone != newPhone) { Phone = newPhone; isUpdated = true; }
        if (newPassportData != null && PassportData != newPassportData) { PassportData = newPassportData; isUpdated = true; }
        return isUpdated;
    }

    public Delivery CreateDelivery(Receiver receiver, AddressText pickupAddress, AddressText deliveryAddress, PaymentMethod paymentMethod)
    {
        if (receiver == null) throw new ArgumentNullException(nameof(receiver));
        var delivery = new Delivery(this, receiver, pickupAddress, deliveryAddress, paymentMethod);
        _deliveries.Add(delivery);
        return delivery;
    }

    /// <summary>
    /// Отмена доставки. Возвращает true, если отмена успешна.
    /// </summary>
    public bool CancelDelivery(Delivery delivery)
    {
        if (delivery == null) throw new ArgumentNullException(nameof(delivery));

        // Проверка принадлежности доставки этому отправителю
        if (delivery.Sender != this)
            throw new AnotherUserEditDeliveryException(delivery, this);  // ← передаём два аргумента

        // Вызываем внутренний метод отмены (он возвращает bool)
        return delivery.CancelDelivery();
    }
}