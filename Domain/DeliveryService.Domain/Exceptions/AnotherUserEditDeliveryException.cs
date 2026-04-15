namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Исключение: попытка редактировать чужую доставку
/// </summary>
public class AnotherUserEditDeliveryException : InvalidOperationException
{
    public Guid DeliveryId { get; }
    public Guid CurrentUserId { get; }
    public Guid OwnerId { get; }

    public AnotherUserEditDeliveryException(Guid deliveryId, Guid currentUserId, Guid ownerId)
        : base($"Пользователь {currentUserId} не может редактировать доставку {deliveryId}. Владелец доставки: {ownerId}. Редактировать доставку может только её владелец.")
    {
        DeliveryId = deliveryId;
        CurrentUserId = currentUserId;
        OwnerId = ownerId;
    }
}