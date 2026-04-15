namespace DeliveryService.Domain.Enums;

/// <summary>
/// Статусы доставки
/// </summary>
public enum DeliveryStatus
{
    /// <summary>Создан</summary>
    Created = 0,

    /// <summary>В пути</summary>
    InTransit = 1,

    /// <summary>Доставлен</summary>
    Delivered = 2,

    /// <summary>Отменён</summary>
    Cancelled = 3
}
