namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Исключение: попытка удалить чужую доставку
/// </summary>
public class AnotherUserDeleteDeliveryException : InvalidOperationException
{
    public Guid DeliveryId { get; }
    public Guid CurrentUserId { get; }
    public Guid OwnerId { get; }

    public AnotherUserDeleteDeliveryException(Guid deliveryId, Guid currentUserId, Guid ownerId)
        : base($"Пользователь {currentUserId} не может удалить доставку {deliveryId}. Владелец доставки: {ownerId}. Удалять доставку может только её владелец.")
    {
        DeliveryId = deliveryId;
        CurrentUserId = currentUserId;
        OwnerId = ownerId;
    }
}