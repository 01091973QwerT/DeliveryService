namespace DeliveryService.Domain.Enums;

/// <summary>
/// Статусы доставки
/// </summary>
public enum DeliveryStatus
{
    Created = 0,      // Создан
    InTransit = 1,    // В пути
    PickedUp = 2,     // Забрали (нельзя отменить)
    Delivered = 3,    // Доставлен
    Cancelled = 4     // Отменён
}