using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Exceptions;

public class AnotherUserEditDeliveryException : InvalidOperationException
{
    public Delivery Delivery { get; }
    public Sender CurrentSender { get; }

    // Конструктор с двумя параметрами (используем в Sender)
    public AnotherUserEditDeliveryException(Delivery delivery, Sender currentSender)
        : base($"Отправитель {currentSender.Name.Value} не может редактировать доставку {delivery.Id}. Владелец: {delivery.Sender.Name.Value}")
    {
        Delivery = delivery;
        CurrentSender = currentSender;
    }
}