using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Доставка не принадлежит отправителю.
/// </summary>
public sealed class DeliveryNotBelongSenderException(Sender sender, Delivery delivery, string actionDescription)
    : InvalidOperationException(
        $"Отправитель «{sender.Name.Value}» (id = {sender.Id}) не может выполнить «{actionDescription}» " +
        $"для доставки id = {delivery.Id}, так как она ему не принадлежит.")
{
    public Sender Sender => sender;
    public Delivery Delivery => delivery;
    public string ActionDescription => actionDescription;
}
