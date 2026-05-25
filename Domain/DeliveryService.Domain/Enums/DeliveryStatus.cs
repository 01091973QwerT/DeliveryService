namespace DeliveryService.Domain.Enums;

/// <summary>
/// Статус доставки.
/// </summary>
public enum DeliveryStatus
{
    /// <summary>Создана, ожидает обработки.</summary>
    Pending,
    /// <summary>В процессе доставки.</summary>
    InProgress,
    /// <summary>Доставлена.</summary>
    Delivered,
    /// <summary>Отменена.</summary>
    Cancelled
}
