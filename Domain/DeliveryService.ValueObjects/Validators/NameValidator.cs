using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Exceptions;

namespace DeliveryService.ValueObjects.Validators;

public class NameValidator : IValidator<string>
{
    public static int MAX_LENGTH => 255;
    public static int MIN_LENGTH => 2;

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
