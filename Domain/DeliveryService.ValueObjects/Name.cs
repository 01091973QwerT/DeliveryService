using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Value Object: Имя (отправителя или получателя)
/// </summary>
public class Name(string name) : ValueObject<string>(new NameValidator(), name);
