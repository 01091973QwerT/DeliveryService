using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Repositories.Abstractions.Base;

namespace DeliveryService.Domain.Repositories.Abstractions;

/// <summary>
/// Интерфейс репозитория для работы с доставками
/// </summary>
public interface IDeliveryRepository : IRepository<Delivery, Guid>
{
    /// <summary>
    /// Получить все доставки отправителя
    /// </summary>
    Task<IReadOnlyList<Delivery>> GetBySenderIdAsync(Guid senderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все доставки получателя
    /// </summary>
    Task<IReadOnlyList<Delivery>> GetByReceiverIdAsync(Guid receiverId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все доставки с определенным статусом
    /// </summary>
    Task<IReadOnlyList<Delivery>> GetByStatusAsync(DeliveryStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все доставки с определенным статусом оплаты
    /// </summary>
    Task<IReadOnlyList<Delivery>> GetByPaymentStatusAsync(PaymentStatus paymentStatus, CancellationToken cancellationToken = default);
}
