using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Exceptions;

namespace DeliveryService.ValueObjects.Validators;

/// <summary>
/// Валидатор для серии паспорта (ровно 4 символа)
/// </summary>
public class PassportSeriesValidator : IValidator<string>
{
    public static int EXACT_LENGTH => 4;

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));

        if (value.Length != EXACT_LENGTH)
            throw new FormatException($"Серия паспорта \"{value}\" должна содержать ровно {EXACT_LENGTH} символа. Сейчас: {value.Length}");
    }
}
