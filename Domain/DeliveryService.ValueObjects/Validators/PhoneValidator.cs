using DeliveryService.ValueObjects.Base;
using DeliveryService.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace DeliveryService.ValueObjects.Validators;

/// <summary>
/// Валидатор для номера телефона
/// </summary>
public class PhoneValidator : IValidator<string>
{
    private static readonly Regex PhoneRegex = new Regex(@"^\+?[0-9]{10,15}$", RegexOptions.Compiled);

    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));  // ← исправлено!

        if (!PhoneRegex.IsMatch(value))
            throw new FormatException($"Номер телефона \"{value}\" имеет неверный формат. Должен содержать 10-15 цифр, опционально начинаться с +");
    }
}
