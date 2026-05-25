using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace DeliveryService.ValueObjects.Validators;

/// <summary>
/// Валидатор номера паспорта (6 цифр).
/// </summary>
public class PassportNumberValidator : IValidator<string>
{
    public static int EXACT_LENGTH => 6;
    private static readonly Regex DigitsOnly = new(@"^\d{6}$", RegexOptions.Compiled);

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));

        if (!DigitsOnly.IsMatch(value))
            throw new FormatException("Passport number must be exactly 6 digits.");
    }
}
