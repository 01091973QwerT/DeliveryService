namespace DeliveryService.Domain.Enums;

/// <summary>
/// Статусы оплаты
/// </summary>
public enum PaymentStatus
{
    /// <summary>Ожидает оплаты</summary>
    Pending = 0,

    /// <summary>Оплачен</summary>
    Paid = 1,

    /// <summary>Возвращен</summary>
    Refunded = 2
}
