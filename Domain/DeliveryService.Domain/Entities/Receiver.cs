using DeliveryService.Domain.Base;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Получатель (receivers).
///
/// Участвует в use-case:
/// - Дает паспортные данные
/// - Уведомление о доставке
/// </summary>
public class Receiver : Entity<Guid>
{
    public Name Name { get; private set; } = default!;
    public Phone Phone { get; private set; } = default!;
    public PassportData? PassportData { get; private set; }

    protected Receiver()
    {
    }

    public Receiver(Name name, Phone phone)
        : this(Guid.NewGuid(), name, phone, null)
    {
    }

    public Receiver(Name name, Phone phone, PassportData? passportData)
        : this(Guid.NewGuid(), name, phone, passportData)
    {
    }

    protected Receiver(Guid id, Name name, Phone phone, PassportData? passportData) : base(id)
    {
        Name = name ?? throw new ArgumentNullValueException(nameof(name));
        Phone = phone ?? throw new ArgumentNullValueException(nameof(phone));
        PassportData = passportData;
    }

    /// <summary>
    /// Use case: «Дает паспортные данные».
    /// Устанавливает или обновляет паспортные данные получателя.
    /// </summary>
    public void SetPassportData(PassportData passportData)
    {
        PassportData = passportData ?? throw new ArgumentNullValueException(nameof(passportData));
    }
}
