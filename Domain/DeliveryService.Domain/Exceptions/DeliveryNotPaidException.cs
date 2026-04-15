namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Исключение: попытка начать доставку без оплаты
/// </summary>
public class DeliveryNotPaidException : InvalidOperationException
{
    public Guid DeliveryId { get; }

    public DeliveryNotPaidException(Guid deliveryId, string paymentStatus)
        : base($"Невозможно начать доставку {deliveryId}. Доставка не оплачена. Текущий статус оплаты: {paymentStatus}")
    {
        DeliveryId = deliveryId;
    }
}
