using DeliveryService.Domain.Base;
using DeliveryService.Domain.Exceptions;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Уведомление о доставке (delivery_notifications).
///
/// Используется для информирования отправителя и получателя
/// о статусе доставки.
/// </summary>
public class DeliveryNotification : Entity<Guid>
{
    public Delivery Delivery { get; private set; } = default!;
    public string Message { get; private set; } = default!;
    public DateTime SentAt { get; private set; }

    protected DeliveryNotification()
    {
    }

    /// <summary>
    /// Создаёт уведомление (используется из <see cref="Delivery"/>).
    /// </summary>
    internal static DeliveryNotification Create(
        Delivery delivery,
        string message,
        DateTime sentAtUtc
    )
    {
        if (delivery is null) throw new ArgumentNullValueException(nameof(delivery));
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullValueException(nameof(message));

        var utc = sentAtUtc.Kind == DateTimeKind.Utc
            ? sentAtUtc
            : DateTime.SpecifyKind(sentAtUtc, DateTimeKind.Utc);

        return new DeliveryNotification(Guid.NewGuid(), delivery, message, utc);
    }

    protected DeliveryNotification(
        Guid id,
        Delivery delivery,
        string message,
        DateTime sentAt
    ) : base(id)
    {
        Delivery = delivery ?? throw new ArgumentNullValueException(nameof(delivery));
        Message = message ?? throw new ArgumentNullValueException(nameof(message));
        SentAt = sentAt;
    }
}
