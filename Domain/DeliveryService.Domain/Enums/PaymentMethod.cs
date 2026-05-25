namespace DeliveryService.Domain.Enums;

/// <summary>
/// Способ оплаты.
/// </summary>
public enum PaymentMethod
{
    /// <summary>Наличные.</summary>
    Cash,
    /// <summary>Банковская карта.</summary>
    Card,
    /// <summary>Онлайн-перевод.</summary>
    Online
}
