using DeliveryService.Domain.Base;
using DeliveryService.Domain.Enums;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

public class Receiver : Entity<Guid>
{
    public Name Name { get; private set; }
    public Phone Phone { get; private set; }
    public PassportData? PassportData { get; private set; }

    private readonly List<Delivery> _deliveries = new();
    public IReadOnlyCollection<Delivery> Deliveries => _deliveries.AsReadOnly();

    protected Receiver() { }

    public Receiver(Name name, Phone phone, PassportData? passportData = null)
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

    /// <summary>
    /// Получить историю доставок получателя
    /// </summary>
    public IReadOnlyCollection<Delivery> GetDeliveriesHistory()
    {
        return _deliveries.OrderByDescending(d => d.CreatedAt).ToList().AsReadOnly();
    }

    internal void AddDelivery(Delivery delivery)
    {
        _deliveries.Add(delivery);
    }
}