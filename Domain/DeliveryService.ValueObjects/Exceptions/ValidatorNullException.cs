namespace DeliveryService.ValueObjects.Exceptions;

/// <summary>
/// Исключение: валидатор не может быть null
/// </summary>
public class ValidatorNullException(string paramName)
    : ArgumentNullException(paramName, $"Валидатор \"{paramName}\" не может быть null. Он обязателен для создания ValueObject.");
