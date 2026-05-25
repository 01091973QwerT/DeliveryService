using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.ValueObjects;

/// <summary>
/// Серия паспорта (4 цифры).
/// </summary>
public sealed class PassportSeries(string series)
    : ValueObject<string>(new PassportSeriesValidator(), (series ?? string.Empty).Trim());
