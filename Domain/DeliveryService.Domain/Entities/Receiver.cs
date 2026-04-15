using DeliveryService.Domain.Base;
using DeliveryService.Domain.Enums;
using DeliveryService.ValueObjects;
using System.Numerics;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Получатель
/// Связи: 
/// - один Receiver может иметь один PassportData
/// - один Receiver может иметь много Delivery
/// </summary>
public class Receiver : Entity<Guid>
{
    // Value Objects
    public Name Name { get; private set; }
    public Phone Phone { get; private set; }

    // Внешний ключ к PassportData
    public Guid? PassportId { get; private set; }

    // Навигационное свойство (связь с паспортными данными)
    public PassportData? PassportData { get; private set; }

    // Коллекция доставок получателя (ICollection)
    private readonly ICollection<Delivery> _deliveries = new List<Delivery>();
    public IReadOnlyCollection<Delivery> Deliveries => _deliveries.ToList().AsReadOnly();

    private Receiver() { }

    public Receiver(Name name, Phone phone, PassportData? passportData = null)
        : base(Guid.NewGuid())
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
    /// Внутренний метод для добавления доставки (используется Sender.CreateDelivery)
    /// </summary>
    internal void AddDelivery(Delivery delivery)
    {
        if (delivery == null)
            throw new ArgumentNullException(nameof(delivery));
        _deliveries.Add(delivery);
    }

    /// <summary>
    /// Use Case: Просмотр истории заказов (получатель)
    /// </summary>
    public IReadOnlyCollection<Delivery> GetDeliveriesHistory()
    {
        return _deliveries.OrderByDescending(d => d.CreatedAt).ToList().AsReadOnly();
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
