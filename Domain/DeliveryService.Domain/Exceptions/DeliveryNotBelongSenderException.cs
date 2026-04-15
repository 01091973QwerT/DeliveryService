namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Исключение: доставка не принадлежит указанному отправителю
/// </summary>
public class DeliveryNotBelongSenderException : InvalidOperationException
{
    public Guid DeliveryId { get; }
    public Guid SenderId { get; }
    public string SenderName { get; }

    public DeliveryNotBelongSenderException(Guid deliveryId, Guid senderId, string senderName)
        : base($"Доставка {deliveryId} не принадлежит отправителю {senderName} (ID: {senderId}). Отправитель может управлять только своими доставками.")
    {
        DeliveryId = deliveryId;
        SenderId = senderId;
        SenderName = senderName;
    }
}
