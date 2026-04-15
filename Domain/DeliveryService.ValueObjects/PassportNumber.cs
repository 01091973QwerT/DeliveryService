using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Value Object: Номер паспорта (ровно 6 символов)
/// </summary>
public class PassportNumber(string number) : ValueObject<string>(new PassportNumberValidator(), number);