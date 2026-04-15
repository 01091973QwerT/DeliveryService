namespace DeliveryService.ValueObjects.Exceptions;

/// <summary>
/// Исключение: значение слишком длинное
/// </summary>
public class ArgumentLongValueException(string paramName, string value, int maxLength)
    : FormatException($"Параметр \"{paramName}\" имеет длину {value.Length}, что превышает максимально допустимую длину {maxLength}")
{
    public string Value => value;
    public int MaxLength => maxLength;
}
