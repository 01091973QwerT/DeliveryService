using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Exceptions;

namespace DeliveryService.ValueObjects.Validators;

/// <summary>
/// Валидатор для номера паспорта (ровно 6 символов)
/// </summary>
public class PassportNumberValidator : IValidator<string>
{
    public static int EXACT_LENGTH => 6;

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));

        if (value.Length != EXACT_LENGTH)
            throw new FormatException($"Номер паспорта \"{value}\" должен содержать ровно {EXACT_LENGTH} символов. Сейчас: {value.Length}");
    }
}
