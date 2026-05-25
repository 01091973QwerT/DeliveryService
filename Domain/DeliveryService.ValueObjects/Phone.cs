using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Телефонный номер.
/// </summary>
public sealed class Phone(string phone)
    : ValueObject<string>(new PhoneValidator(), (phone ?? string.Empty).Trim());
