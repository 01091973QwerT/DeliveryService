namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Исключение: операция требует определенного статуса доставки
/// </summary>
public class InvalidDeliveryStatusException : InvalidOperationException
{
    public InvalidDeliveryStatusException(string message) : base(message) { }
}
