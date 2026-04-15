using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Value Object: Адрес (текстовое представление)
/// </summary>
public class AddressText(string address) : ValueObject<string>(new AddressValidator(), address);
