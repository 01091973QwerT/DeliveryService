using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Другой отправитель пытается удалить чужую доставку.
/// </summary>
public sealed class AnotherUserDeleteDeliveryException(Sender actor, Sender owner, Delivery delivery)
    : InvalidOperationException(
        $"Отправитель «{actor.Name.Value}» (id = {actor.Id}) не может удалить доставку id = {delivery.Id}, " +
        $"так как владельцем является «{owner.Name.Value}» (id = {owner.Id}).")
{
    public Sender Actor => actor;
    public Sender Owner => owner;
    public Delivery Delivery => delivery;
}
