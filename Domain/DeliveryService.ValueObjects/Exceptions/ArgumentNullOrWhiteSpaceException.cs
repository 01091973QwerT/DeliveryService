namespace DeliveryService.ValueObjects.Exceptions;

/// <summary>
/// Исключение: значение null, пустое или состоит только из пробелов
/// </summary>
public class ArgumentNullOrWhiteSpaceException(string paramName)
    : ArgumentNullException(paramName, $"Параметр \"{paramName}\" не может быть null, пустым или состоять только из пробелов.");