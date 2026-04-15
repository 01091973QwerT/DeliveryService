using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Abstractions.Base;

namespace DeliveryService.Domain.Repositories.Abstractions;

/// <summary>
/// Интерфейс репозитория для работы с паспортными данными
/// </summary>
public interface IPassportDataRepository : IRepository<PassportData, Guid>
{
    /// <summary>
    /// Найти паспортные данные по серии и номеру (уникальная комбинация)
    /// </summary>
    Task<PassportData?> GetBySeriesAndNumberAsync(string series, string number, CancellationToken cancellationToken = default);
}