using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace DeliveryService.ValueObjects.Validators;

/// <summary>
/// Валидатор серии паспорта (4 цифры).
/// </summary>
public class PassportSeriesValidator : IValidator<string>
{
    public static int EXACT_LENGTH => 4;
    private static readonly Regex DigitsOnly = new(@"^\d{4}$", RegexOptions.Compiled);

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));

        if (!DigitsOnly.IsMatch(value))
            throw new FormatException("Passport series must be exactly 4 digits.");
    }
}
