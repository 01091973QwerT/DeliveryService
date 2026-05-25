using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Номер паспорта (6 цифр).
/// </summary>
public sealed class PassportNumber(string number)
    : ValueObject<string>(new PassportNumberValidator(), (number ?? string.Empty).Trim());
