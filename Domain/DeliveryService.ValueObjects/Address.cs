using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Адрес (адрес забора или адрес доставки).
/// </summary>
public sealed class Address(string address)
    : ValueObject<string>(new AddressValidator(), (address ?? string.Empty).Trim());
