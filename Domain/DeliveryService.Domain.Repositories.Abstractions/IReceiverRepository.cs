using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Abstractions.Base;

namespace DeliveryService.Domain.Repositories.Abstractions;

/// <summary>
/// Интерфейс репозитория для работы с получателями
/// </summary>
public interface IReceiverRepository : IRepository<Receiver, Guid>
{
    /// <summary>
    /// Найти получателя по номеру телефона (уникальный)
    /// </summary>
    Task<Receiver?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все доставки получателя
    /// </summary>
    Task<IReadOnlyList<Delivery>> GetReceiverDeliveriesAsync(Guid receiverId, CancellationToken cancellationToken = default);
}
