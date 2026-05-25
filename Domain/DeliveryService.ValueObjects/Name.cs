using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Обозначает имя отправителя/получателя.
/// </summary>
public sealed class Name(string name)
    : ValueObject<string>(new NameValidator(), (name ?? string.Empty).Trim());
