using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Другой отправитель пытается редактировать чужую доставку.
/// </summary>
public sealed class AnotherUserEditDeliveryException(Sender actor, Sender owner, Delivery delivery)
    : InvalidOperationException(
        $"Отправитель «{actor.Name.Value}» (id = {actor.Id}) не может редактировать доставку id = {delivery.Id}, " +
        $"так как владельцем является «{owner.Name.Value}» (id = {owner.Id}).")
{
    public Sender Actor => actor;
    public Sender Owner => owner;
    public Delivery Delivery => delivery;
}
