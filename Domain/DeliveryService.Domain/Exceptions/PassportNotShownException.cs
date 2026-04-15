namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Исключение: попытка получить доставку без демонстрации паспорта
/// </summary>
public class PassportNotShownException : InvalidOperationException
{
    public Guid DeliveryId { get; }

    public PassportNotShownException(Guid deliveryId)
        : base($"Невозможно получить доставку {deliveryId}. Паспорт не был показан курьеру. Получатель должен предъявить паспорт.")
    {
        DeliveryId = deliveryId;
    }
}
