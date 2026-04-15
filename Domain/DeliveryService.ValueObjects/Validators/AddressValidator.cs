using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Exceptions;

namespace DeliveryService.ValueObjects.Validators;

/// <summary>
/// Валидатор для адреса
/// </summary>
public class AddressValidator : IValidator<string>
{
    public static int MAX_LENGTH => 500;
    public static int MIN_LENGTH => 5;

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));

        if (value.Length > MAX_LENGTH)
            throw new ArgumentLongValueException(nameof(value), value, MAX_LENGTH);

        if (value.Length < MIN_LENGTH)
            throw new ArgumentShortValueException(nameof(value), value, MIN_LENGTH);
    }
}
