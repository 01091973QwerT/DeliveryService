using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Abstractions.Base;

namespace DeliveryService.Domain.Repositories.Abstractions;

/// <summary>
/// Интерфейс репозитория для работы с отправителями
/// </summary>
public interface ISenderRepository : IRepository<Sender, Guid>
{
    /// <summary>
    /// Найти отправителя по номеру телефона (уникальный)
    /// </summary>
    Task<Sender?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все доставки отправителя
    /// </summary>
    Task<IReadOnlyList<Delivery>> GetSenderDeliveriesAsync(Guid senderId, CancellationToken cancellationToken = default);
}
