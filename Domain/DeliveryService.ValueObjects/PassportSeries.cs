using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Value Object: Серия паспорта (ровно 4 символа)
/// </summary>
public class PassportSeries(string series) : ValueObject<string>(new PassportSeriesValidator(), series);