using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace DeliveryService.ValueObjects.Validators;

public class PhoneValidator : IValidator<string>
{
    public static int MAX_LENGTH => 20;
    public static int MIN_LENGTH => 5;
    private static readonly Regex PhonePattern = new(@"^[\d\s\+\-\(\)]+$", RegexOptions.Compiled);

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));

        if (value.Length > MAX_LENGTH)
            throw new ArgumentLongValueException(nameof(value), value, MAX_LENGTH);

        if (value.Length < MIN_LENGTH)
            throw new ArgumentShortValueException(nameof(value), value, MIN_LENGTH);

        if (!PhonePattern.IsMatch(value))
            throw new FormatException("Phone number must contain only digits, spaces, and symbols: + - ( )");
    }
}
