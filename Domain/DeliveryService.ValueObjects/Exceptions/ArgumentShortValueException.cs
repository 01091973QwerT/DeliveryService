namespace DeliveryService.ValueObjects.Exceptions;

/// <summary>
/// Исключение: значение слишком короткое
/// </summary>
public class ArgumentShortValueException(string paramName, string value, int minLength)
    : FormatException($"Параметр \"{paramName}\" имеет длину {value.Length}, что меньше минимально допустимой длины {minLength}")
{
    public string Value => value;
    public int MinLength => minLength;
}
