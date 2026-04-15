namespace DeliveryService.Domain.Enums;

/// <summary>
/// Способы оплаты
/// </summary>
public enum PaymentMethod
{
    /// <summary>Онлайн картой</summary>
    Online = 0,

    /// <summary>Наличными при получении</summary>
    Cash = 1,

    /// <summary>Терминалом при получении</summary>
    Terminal = 2,

    /// <summary>Корпоративный счет</summary>
    Corporate = 3
}
