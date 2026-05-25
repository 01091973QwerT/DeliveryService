using DeliveryService.Domain.Base;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Отправитель (senders).
///
/// Хранит свои доставки и выполняет действия из use-case диаграммы:
/// - Формирует доставку из пункта А в пункт Б
/// - Выбор оплаты
/// - Дает паспортные данные
/// - Просмотр заказов или повторный заказ
/// </summary>
public class Sender : Entity<Guid>
{
    private readonly ICollection<Delivery> _deliveries = [];

    public Name Name { get; private set; } = default!;
    public Phone Phone { get; private set; } = default!;
    public PassportData? PassportData { get; private set; }

    public IReadOnlyCollection<Delivery> Deliveries => _deliveries.ToList().AsReadOnly();

    protected Sender()
    {
    }

    public Sender(Name name, Phone phone)
        : this(Guid.NewGuid(), name, phone, null)
    {
    }

    public Sender(Name name, Phone phone, PassportData? passportData)
        : this(Guid.NewGuid(), name, phone, passportData)
    {
    }

    protected Sender(Guid id, Name name, Phone phone, PassportData? passportData) : base(id)
    {
        Name = name ?? throw new ArgumentNullValueException(nameof(name));
        Phone = phone ?? throw new ArgumentNullValueException(nameof(phone));
        PassportData = passportData;
    }

    /// <summary>
    /// Use case: «Дает паспортные данные».
    /// Устанавливает или обновляет паспортные данные отправителя.
    /// </summary>
    public void SetPassportData(PassportData passportData)
    {
        PassportData = passportData ?? throw new ArgumentNullValueException(nameof(passportData));
    }

    /// <summary>
    /// Use case: «Формирует доставку из пункта А в пункт Б».
    /// Создаёт новую доставку и добавляет её в коллекцию отправителя.
    /// </summary>
    public Delivery CreateDelivery(
        Receiver receiver,
        Address pickupAddress,
        Address deliveryAddress,
        Enums.PaymentMethod paymentMethod,
        bool receiverPassportShown,
        DateTime createdAtUtc
    )
    {
        if (receiver is null) throw new ArgumentNullValueException(nameof(receiver));
        if (pickupAddress is null) throw new ArgumentNullValueException(nameof(pickupAddress));
        if (deliveryAddress is null) throw new ArgumentNullValueException(nameof(deliveryAddress));

        var delivery = new Delivery(
            this,
            receiver,
            pickupAddress,
            deliveryAddress,
            paymentMethod,
            receiverPassportShown,
            createdAtUtc
        );
        _deliveries.Add(delivery);
        return delivery;
    }

    /// <summary>
    /// Use case: «Просмотр заказов».
    /// Возвращает все доставки отправителя.
    /// </summary>
    public IReadOnlyCollection<Delivery> ViewDeliveries()
    {
        return Deliveries;
    }

    /// <summary>
    /// Use case: «Повторный заказ».
    /// Создаёт новую доставку на основе существующей.
    /// </summary>
    public Delivery RepeatDelivery(Sender actor, Delivery existingDelivery, DateTime createdAtUtc)
    {
        if (actor is null) throw new ArgumentNullValueException(nameof(actor));
        if (existingDelivery is null) throw new ArgumentNullValueException(nameof(existingDelivery));

        if (!ReferenceEquals(actor, this))
            throw new DeliveryNotBelongSenderException(actor, existingDelivery, "Повторный заказ");

        if (!_deliveries.Contains(existingDelivery))
            throw new InvalidOperationException(
                $"Доставка id = {existingDelivery.Id} не найдена среди доставок отправителя «{Name.Value}» (id = {Id})."
            );

        return CreateDelivery(
            existingDelivery.Receiver,
            existingDelivery.PickupAddress,
            existingDelivery.DeliveryAddress,
            existingDelivery.PaymentMethod,
            existingDelivery.ReceiverPassportShown,
            createdAtUtc
        );
    }

    /// <summary>
    /// Отмена доставки.
    /// </summary>
    public bool CancelDelivery(Sender actor, Delivery delivery)
    {
        if (actor is null) throw new ArgumentNullValueException(nameof(actor));
        if (delivery is null) throw new ArgumentNullValueException(nameof(delivery));

        if (!ReferenceEquals(actor, this))
            throw new AnotherUserDeleteDeliveryException(actor, this, delivery);

        if (!_deliveries.Contains(delivery))
            throw new InvalidOperationException(
                $"Доставка id = {delivery.Id} не найдена среди доставок отправителя «{Name.Value}» (id = {Id})."
            );

        return delivery.Cancel(actor);
    }

    /// <summary>
    /// Use case: «Выбор оплаты».
    /// Изменяет способ оплаты для доставки.
    /// </summary>
    public bool ChangePaymentMethod(Sender actor, Delivery delivery, Enums.PaymentMethod newPaymentMethod)
    {
        if (actor is null) throw new ArgumentNullValueException(nameof(actor));
        if (delivery is null) throw new ArgumentNullValueException(nameof(delivery));

        if (!ReferenceEquals(actor, this))
            throw new AnotherUserEditDeliveryException(actor, this, delivery);

        if (!_deliveries.Contains(delivery))
            throw new InvalidOperationException(
                $"Доставка id = {delivery.Id} не найдена среди доставок отправителя «{Name.Value}» (id = {Id})."
            );

        return delivery.ChangePaymentMethod(actor, newPaymentMethod);
    }
}
