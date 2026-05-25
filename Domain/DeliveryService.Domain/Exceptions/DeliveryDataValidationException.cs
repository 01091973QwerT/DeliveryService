namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Ошибка валидации данных доставки.
/// </summary>
public class DeliveryDataValidationException(string paramName, object? value)
    : ArgumentException($"Invalid delivery data: '{paramName}' value '{value}' is not allowed.")
{
    public string DataParamName { get; } = paramName;
    public object? Value { get; } = value;
}
