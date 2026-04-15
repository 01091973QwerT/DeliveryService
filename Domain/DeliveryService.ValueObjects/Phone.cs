using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Value Object: Номер телефона
/// </summary>
public class Phone(string phone) : ValueObject<string>(new PhoneValidator(), phone);
